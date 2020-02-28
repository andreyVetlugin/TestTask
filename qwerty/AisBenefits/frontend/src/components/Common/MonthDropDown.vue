<template>
  <DropDown
    class="month_dropdown"
    ref="dropdown"
    :value="value"
    @input="input"
    tiled
    :title="title"
    :list="list"
    :disabled="disabled"
  ></DropDown>
</template>

<style>
.month_dropdown {
  width: 120px;
}
.month_dropdown .common-button {
  display: none;
}
.month_dropdown .text-input {
  width: 100%;
  padding: 23px;
}
.month_dropdown.dropdown.tiled ul {
  margin: 8px -24px;
  width: calc(100% + 48px);
}
</style>


<script lang="ts">
import Vue from 'vue';
import DropDown from '@/components/DropDown.vue';
export default Vue.extend({
  data() {
    return {
      list: [
        { value: '01', title: 'Январь' },
        { value: '02', title: 'Февраль' },
        { value: '03', title: 'Март' },
        { value: '04', title: 'Апрель' },
        { value: '05', title: 'Май' },
        { value: '06', title: 'Июнь' },
        { value: '07', title: 'Июль' },
        { value: '08', title: 'Август' },
        { value: '09', title: 'Сентябрь' },
        { value: '10', title: 'Октябрь' },
        { value: '11', title: 'Ноябрь' },
        { value: '12', title: 'Декабрь' },
      ],
    };
  },
  props: {
    value: String,
    title: String,
    disabled: Boolean,
  },
  mounted() {
    const self = this;
    const int = setInterval(() => {
      if (self.$refs.dropdown) {
        (self.$refs.dropdown as any).findDisplayInList();
        clearInterval(int);
      }
    }, 100);
  },
  methods: {
    input(value: string) {
      this.$emit('input', value);
    },
  },
  components: {
    DropDown,
  },
});
</script>
