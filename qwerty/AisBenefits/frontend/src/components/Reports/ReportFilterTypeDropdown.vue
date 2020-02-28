<template>
  <DropDown class="report_row_type_dropdown" :list="list" :value="val" @input="input"></DropDown>
</template>

<style>
.report_row_type_dropdown.dropdown {
  width: 64px;
}
.report_row_type_dropdown .common-button {
  display: none;
}
.report_row_type_dropdown .text-input {
  width: 64px;
  padding-left: 0;
  padding-right: 0;
  text-align: center;
}
.report_row_type_dropdown ul li {
  padding-left: 0;
  padding-right: 0;
  text-align: center;
}
</style>

<script lang="ts">
import Vue from 'vue';
import DropDown from '@/components/DropDown.vue';
import { FilterRules } from '@/models/reports/reportRow.ts';
export default Vue.extend({
  data() {
    return {
      val: '-1',
      list: [
        { value: '0', title: '=' },
        { value: '1', title: '>' },
        { value: '2', title: '<' },
        { value: '3', title: '<>' },
      ],
    };
  },
  mounted() {
    this.val = this.value.toString();
  },
  props: {
    value: Number,
  },
  watch: {
    value() {
      this.val = this.value.toString();
    },
  },
  methods: {
    input(value: string) {
      this.$emit('input', parseInt(value, 10));
    },
  },
  components: {
    DropDown,
  },
});
</script>
