<template>
  <div class="dropdown" :class="{ expanded: expanded, tiled: tiled }">
    <h5 v-if="title" v-text="title"></h5>
    <div class="dropdown-container" :class="{ disabled: disabled }" @click="onclick">
      <div class="dropdown-input">
        <input
          ref="input"
          class="text-input"
          v-model="display"
          :placeholder="placeholder || 'не выбрано'"
          @focus="onfocus"
          @blur="onblur"
          :readonly="!editable || disabled"
        >
        <ArrowButton v-show="!disabled" noBackground></ArrowButton>
      </div>
      <ul class="dropdown-list shadow_medium" :class="{dropup: dropup || dropUp}" v-show="expanded">
        <template v-if="tiled">
          <li
            v-for="option in filteredList"
            :key="option.value"
            :class="{ selected: option.value === value}"
            @click="select(option.value)"
            v-text="formatTitle(option.title)"
          ></li>
        </template>
        <Scrollable v-else class="dropdown_scroll" hideScroll>
          <li
            v-for="option in filteredList"
            :key="option.value"
            :class="{ selected: option.value === value}"
            @click="select(option.value)"
            v-text="formatTitle(option.title)"
          ></li>
        </Scrollable>
      </ul>
    </div>
  </div>
</template>

<style>
.dropdown {
  display: inline-block;
  box-sizing: border-box;
  background-color: transparent;
}
.dropdown .scrollable-container.dropdown_scroll {
  max-height: 320px;
  max-width: 100%;
  margin: 0 !important;
}
.dropdown-container {
  display: inline-block;
  box-sizing: border-box;
  background-color: transparent;
  border: 1px solid #6253a2;
  border-radius: 4px;
  margin: 0;
  height: 64px;
  width: 100%;
  position: relative;
  background-color: #503f96;
}
.dropdown-container:hover {
  border-color: #d49e35;
}

.dropdown-container:focus {
  border-color: #8a7ec0;
}

.dropdown-container.error {
  border-color: red;
}
.dropdown-container:active {
  border-color: #b7882e;
}
.dropdown-container.disabled {
  border-color: #6253a2;
}

.dropdown-input {
  display: inline-flex;
  align-items: center;
  height: 64px;
  width: 100%;
}

.dropdown h5 {
  line-height: 1em;
  margin: 0 0 12px;
  font-size: 12pt;
}
.dropdown .text-input {
  color: #ffffff;
  font-size: 12pt;
  line-height: 18pt;
  width: calc(100% - 64px);
  height: 64px;
  padding: 23px;
  padding-right: 0;
  box-sizing: border-box;
  border-color: transparent;
  background-color: transparent;
  margin: 0;
  border-radius: 4px;
  font-family: "Rubik-Light";
  cursor: default;
}
.dropdown .text-input:focus,
.dropdown .text-input:active,
.dropdown .text-input:hover {
  border-color: transparent;
}

.dropdown .arrowed_button {
  transition: transform 0.3s ease-in-out;
}
.expanded .arrowed_button {
  transform: rotate(180deg);
}

.dropdown-list {
  position: absolute;
  padding: 0;
  margin: 0;
  width: 100%;
  border-radius: 4px;
  border: 1px solid #6253a2;
  background-color: #503f96;
  z-index: 10;
}
.dropdown-list.dropup {
  bottom: calc(100% + 8px);
}

.dropdown-list li {
  list-style: none;
  padding: 24px;
  font-size: 12pt;
  color: #fff;
  width: 100%;
  box-sizing: border-box;
}
.dropdown-list li:first-child {
  border-top-left-radius: 4px;
  border-top-right-radius: 4px;
}
.dropdown-list li:last-child {
  border-bottom-left-radius: 4px;
  border-bottom-right-radius: 4px;
}
.dropdown-list li.selected {
  background-color: #5d4ca3;
}

.dropdown.tiled ul {
  margin: 8px auto;
  display: flex;
  flex-wrap: wrap;
}
.dropdown.tiled li {
  text-align: center;
  padding: 24px 0;
  flex: 1 1 33%;
  overflow: hidden;
  border: 1px solid #6253a2;
  border-radius: 0;
}

.dropdown-list li:hover {
  background-color: #d49e35;
}
.dropdown-list li:active {
  background-color: #b7882e;
}
</style>

<script lang="ts">
import Vue from 'vue';
import ArrowButton from '@/components/Common/Buttons/ArrowButton.vue';
import Scrollable from './Scrollable.vue';
export default Vue.extend({
  props: {
    dropup: Boolean,
    value: String,
    title: String,
    list: Array,
    editable: Boolean,
    tiled: Boolean,
    disabled: Boolean,
    placeholder: String,
  },
  data() {
    return {
      expanded: false,
      dropUp: false,
      display: ' ',
    };
  },
  mounted() {
    this.dropUp = ((this.$el.offsetTop > 320) && ((document.body.clientHeight - this.$el.offsetTop) < 320));
    this.findDisplayInList();
  },
  updated() {
    this.dropUp = ((this.$el.offsetTop > 320) && ((document.body.clientHeight - this.$el.offsetTop) < 320));
  },
  computed: {
    filteredList(): Array<{}> {
      return this.editable
        ? (this.list as any[]).filter((el: any) => {
          return el.title.toLowerCase().indexOf((this.display as string).toLowerCase()) >= 0;
        })
        : this.list as Array<{}>;
    },
  },
  watch: {
    value() {
      this.findDisplayInList();
    },
    list() {
      this.findDisplayInList();
    },
  },
  methods: {
    findDisplayInList() {
      if (this.$props.list && this.$props.list.length) {
        const option = this.$props.list.find((elem: any) =>
          elem.value === this.$props.value,
        );
        this.$data.display = option && option.title ? option.title : '';
      }
    },
    select(value: string) {
      if (this.$props.disabled) {
        return false;
      }
      this.$emit('input', value);
    },
    onclick(e) {
      const input = e.currentTarget.getElementsByTagName('input')[0];
      this.$data.expanded
        ? input.blur()
        : input.focus();
    },
    onfocus() {
      if (this.$props.disabled) {
        return false;
      }
      if (this.$props.editable) {
        this.$data.display = '';
      }
      setTimeout(() => { this.$data.expanded = true; }, 200);
    },
    onblur() {
      setTimeout(() => {
        this.$data.expanded = false;
        if (this.$props.editable) {
          (this as any).findDisplayInList();
        }
      }, 200);
    },
    formatTitle(title: string): string {
      if (!title || !this.$props.tiled || title.length < 5) {
        return title;
      }
      return title.substring(0, 3);
    },
  },
  components: {
    ArrowButton,
    Scrollable,
  },
});
</script>
