<template>
  <div class="prompt_input_container" :class="{ expanded: expanded }">
    <LabeledInput
      :title="title"
      :value="value"
      :disabled="disabled"
      :placeholder="placeholder"
      :minlength="minlength"
      :maxlength="maxlength"
      :required="required"
      :pattern="pattern"
      :deniedReg="deniedReg"
      @input="oninput"
      @focus="onfocus"
      @blur="onblur"
    ></LabeledInput>
    <ul class="prompt-list shadow_medium" :class="{dropup: dropup || dropUp}" v-show="expanded && promptList.length">
      <Scrollable class="prompt_scroll" hideScroll>
        <li
          v-for="prompt in promptList"
          :key="prompt.value"
          @click="select(prompt)"
          v-text="prompt.title"
        ></li>
      </Scrollable>
    </ul>
  </div>
</template>

<style>
.prompt_input_container {
  display: inline-block;
  box-sizing: border-box;
  background-color: transparent;
  position: relative;
  font-size: 0;
}
.prompt_input_container .labeled-input {
  width: 100%;
}
.prompt-list {
  position: absolute;
  padding: 0;
  margin: 0;
  width: 100%;
  border-radius: 4px;
  border: 1px solid #6253a2;
  background-color: #503f96;
  z-index: 10;
}
.prompt-list.dropup {
  bottom: calc(100% + 8px);
}
.prompt-list .scrollable-container.prompt_scroll {
  max-height: 320px;
  max-width: 100%;
  margin: 0 !important;
}
.prompt-list li {
  list-style: none;
  padding: 24px;
  font-size: 12pt;
  color: #fff;
  width: 100%;
  box-sizing: border-box;
}
.prompt-list li:first-child {
  border-top-left-radius: 4px;
  border-top-right-radius: 4px;
}
.prompt-list li:last-child {
  border-bottom-left-radius: 4px;
  border-bottom-right-radius: 4px;
}
</style>


<script lang="ts">
import Vue from 'vue';
import LabeledInput from './LabeledInput.vue';
import Scrollable from './Scrollable.vue';
export default Vue.extend({
  props: {
    dropup: Boolean,
    value: String,
    title: String,
    prompts: Array,
    disabled: Boolean,
    placeholder: String,
    minlength: Number,
    maxlength: Number,
    required: Boolean,
    pattern: String,
    deniedReg: RegExp,
  },
  data() {
    return {
      expanded: false,
      dropUp: false,
    };
  },
  mounted() {
    this.dropUp = ((this.$el.offsetTop > 320) && ((document.body.clientHeight - this.$el.offsetTop) < 320));
  },
  updated() {
    this.dropUp = ((this.$el.offsetTop > 320) && ((document.body.clientHeight - this.$el.offsetTop) < 320));
  },
  computed: {
    promptList(): Array<{}> {
      return this.value && this.prompts
        ? (this.prompts as any[]).filter((el: any) => {
          return el.title.toLowerCase().indexOf((this.value as string).toLowerCase()) >= 0;
        })
        : [];
    },
  },
  methods: {
    select(prompt: any) {
      if (this.$props.disabled) {
        return false;
      }
      this.$emit('input', prompt.title);
      this.$emit('change', prompt.value);
    },
    onclick(e: any) {
      const input = e.currentTarget.getElementsByTagName('input')[0];
      this.$data.expanded
        ? input.blur()
        : input.focus();
    },
    onfocus() {
      if (this.$props.disabled) {
        return false;
      }
      setTimeout(() => { this.$data.expanded = true; }, 200);
    },
    onblur() {
      setTimeout(() => {
        this.$data.expanded = false;
      }, 200);
    },
    oninput(val: string) {
      this.$emit('input', val);
    },
  },
  components: {
    LabeledInput,
    Scrollable,
  },
});
</script>
