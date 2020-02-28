<template>
  <div class="labeled-input">
    <h5 v-if="title" v-text="title"></h5>
    <input
      ref="input"
      class="text-input"
      :class="{ error: !isValid }"
      :value="value"
      :type="type"
      :min="min"
      :max="max"
      :minlength="minlength"
      :maxlength="maxlength"
      :placeholder="placeholder"
      :disabled="disabled"
      @keyup.left="onclick"
      @keyup.right="onclick"
      @keypress.enter="emit"
      @click="onclick"
      @input="oninput"
      @change="onchange"
      @focus="onfocus"
      @blur="onblur"
    />
  </div>
</template>


<style>
.labeled-input {
  display: inline-block;
  box-sizing: border-box;
  overflow: hidden;
}
.labeled-input h5 {
  line-height: 1em;
  margin: 0 0 12px;
  font-size: 12pt;
}
.labeled-input .text-input {
  width: 100%;
  margin: 0;
  transition: border 0.1s linear, border-color 0.1s linear;
  outline: none;
  background-color: transparent;
  border: 1px solid #6253a2;
  border-radius: 4px;
  box-sizing: border-box;
  height: 64px;
  padding: 24px 23px 23px;
  font-size: 12pt;
  font-family: "Rubik-Light";
  color: #ffffff;
  line-height: 16pt;
}

.text-input:hover {
  border-color: #d49e35;
}

.text-input:focus {
  border-color: #8a7ec0;
}

.text-input.error {
  border-color: red;
}

.text-input:active {
  border-color: #b7882e;
}

.text-input[disabled] {
  opacity: 0.5;
  background-color: #2c205e;
  color: #c6bcee;
}
</style>

<script lang="ts">
import Vue from 'vue';
import { formatMoney } from '@/models/utils/stringBuilder';
import InputMasker from '@/models/utils/inputMasker';
import DateValidator from '@/models/utils/validators/dateValidator';
import SnilsValidator from '@/models/utils/validators/snilsValidator';
import Validator from '@/models/utils/validators/validator';
import { floatParse } from '@/models/utils/calculations';
export default Vue.extend({
  props: {
    title: String,
    value: String,
    placeholder: String,
    type: String,
    required: Boolean,
    pattern: String,
    deniedReg: RegExp,
    min: String,
    max: String,
    minlength: Number,
    maxlength: Number,
    disabled: Boolean,
    noclear: Boolean,
  },
  data() {
    return {
      validator: {} as Validator,
      isValid: true,
      masker: {
        input(e: KeyboardEvent) {
          if (!e || !e.target) {
            return '';
          }
          return (e.target as HTMLInputElement).value;
        },
        mask(value: string) {
          return value;
        },
        validate(value: string): boolean {
          return true;
        },
        getCaret(pos?: number, backward = false, clicked = false) {
          return pos || 0;
        },
      },
    };
  },
  mounted() {
    const input = this.$refs.input as HTMLInputElement;
    input.value = this.value || '';
    if (this.pattern) {
      switch (this.pattern) {
        case 'money':
          const self = this;
          this.masker.mask = (value: string) => {
            // const input = self.$refs.input as HTMLInputElement;
            if (!value.includes('.') && value.length > 2) {
              value = value.substring(0, value.length - 2);
            }
            const pre = floatParse(value.replace('..', '.'));
            const result = pre ? formatMoney(pre) : '';
            const selectionStart = (input.selectionStart || 0);
            let caret = input.value.length - selectionStart;
            if  (value[selectionStart - 1] === '.' ||
            (value[selectionStart] === ' ' && value[selectionStart - 1] === ' ')) {
              caret--;
            }
            if (caret && caret > 2) {
              let newCaret = result.length - caret;
              if (result[newCaret - 1] === ' ' && (result.length > value.length)) {
                newCaret--;
              }
              input.selectionStart = newCaret;
              input.selectionEnd = newCaret;
            }
            return result;
          };
          this.masker.getCaret = (pos?: number, backward = false, clicked = false) => {
            pos = pos || 0;
            return pos;
          };
          break;
        case 'snils':
          this.validator = new SnilsValidator();
          this.masker = new InputMasker('999-999-999 99', false);
          this.$emit('input', this.masker.mask(this.value));
          break;
        case 'guid':
          this.masker = new InputMasker('hhhhhhhh-hhhh-hhhh-hhhh-hhhhhhhhhhhh', false);
          this.$emit('input', this.masker.mask(this.value));
          break;
        case '99.99.9999':
        case 'date':
          this.validator = new DateValidator();
          this.masker = new InputMasker('99.99.9999', false);
          this.$emit('input', this.masker.mask(this.value));
          break;
        default:
          this.masker = new InputMasker(this.pattern, false);
          this.$emit('input', this.masker.mask(this.value));
      }
    }
  },
  watch: {
    maxlength(val) {
      this.$emit('input', (this.value || '').substring(0, val));
    },
    isValid(val) {
      this.$emit('validated', val);
    },
  },
  methods: {
    oninput(e: KeyboardEvent) {
      if (!e || !e.target) {
        return;
      }

      const target = (e.target as HTMLInputElement);
      let val = target.value;

      if (this.maxlength && val.length > this.maxlength) {
        val = val.substring(0, this.maxlength);
        target.value = val;
      }

      if (this.masker) {
        if (this.pattern && (this.pattern === 'date' || this.pattern === '99.99.9999')) {
          const oldVals = this.value.split('.');
          const newVals = val.split('.');
          const index = Math.min(Math.floor((target.selectionStart || 0) / 3), 2);
          oldVals[index] = newVals[index] + '0';
          val = `${(oldVals[0] || '').substr(0, 2)}.${(oldVals[1] || '').substr(0, 2)}.${(oldVals[2] || '').substr(0, 4)}`;
        }
        val = this.masker.mask(val);
      }

      if (this.deniedReg && val) {
        val = val.replace(this.deniedReg, '');
      }

      if (target.type !== 'number') {
        const caret = this.masker.getCaret(target.selectionStart || 0,
          (e as any).inputType === 'deleteContentBackward');
        target.value = val;
        if (caret >= 0) {
          target.selectionStart = caret;
          target.selectionEnd = caret;
        }
      }

      this.$emit('input', val);
    },
    onchange(e: KeyboardEvent) {
      if (e && e.target) {
        this.$emit('change', (e.target as HTMLInputElement).value);
      }
    },
    onblur(e: any) {
      this.$emit('blur', e);
      if (!this.noclear && !this.masker.validate(this.value)) {
        this.$emit('input', '');
        this.isValid = !this.required;
        return;
      }
      if (this.required && !this.value) {
        this.isValid = false;
        return;
      }
      if ((this.minlength && this.value.length < this.minlength)) {
        this.isValid = false;
        return;
      }
      if (this.pattern === 'money' && !this.value) {
        this.$emit('input', '0.00');
      }
      if (this.validator && this.validator.validate && typeof this.validator.validate === 'function') {
        this.isValid = this.validator.validate(this.value);
        return;
      }
      this.isValid = true;
    },
    onfocus(e: any) {
      this.$emit('focus', e);
      if (e && e.target) {
        const target = (e.target as HTMLInputElement);
        if (/^[\d.]*$/g.test(target.value) && !parseFloat(target.value)) {
          (e.target as HTMLInputElement).value = '';
        }
      }
      this.masker.mask(this.value);
      this.isValid = true;
    },
    onclick(e: any) {
      if (e && e.target && e.target.type !== 'number') {
        const caret = this.masker.getCaret(e.target.selectionStart, e.keyCode === 37, true);
        if (caret >= 0) {
          e.target.selectionStart = caret;
          e.target.selectionEnd = caret;
        }
      }
    },
    emit(e: Event) {
      this.$emit(e.type, e);
    },
  },
});
</script>
