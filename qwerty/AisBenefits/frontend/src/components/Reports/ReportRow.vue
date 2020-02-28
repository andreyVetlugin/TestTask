<template>
  <div class="report_row">
    <CheckBox v-model="row.selected" :title="row.title"></CheckBox>
    <CheckBox v-if="row.canFiltered" v-model="row.filtered" title="фильтрация"></CheckBox>
    <div v-show="row.filtered" class="filter_block">
      <template v-if="row.variants && row.variants.length">
        <DropDown editable v-model="row.filterValue" :list="row.variants"></DropDown>
      </template>
      <template v-else>
        <ReportFilterTypeDropdown v-model="row.filterRule"></ReportFilterTypeDropdown>
        <LabeledInput :pattern="row.isNumeric ? 'money' : ''" v-model="row.filterValue"></LabeledInput>
      </template>
    </div>
  </div>
</template>

<style>
.report_row {
  display: flex;
  flex-wrap: nowrap;
  justify-content: flex-start;
  align-content: center;
}
.report_row .checkbox {
  flex: 0 0 30%;
}
.report_row .filter_block {
  display: flex;
  align-content: center;
  flex: 0 0 40%;
}
.report_row .filter_block .labeled-input .text-input {
  background-color: #503f96;
}
</style>

<script lang="ts">
import Vue from 'vue';
import LabeledInput from '@/components/LabeledInput.vue';
import DropDown from '@/components/DropDown.vue';
import CheckBox from '@/components/Common/CheckBox.vue';
import ReportFilterTypeDropdown from '@/components/Reports/ReportFilterTypeDropdown.vue';
import ReportRow from '@/models/reports/reportRow.ts';
export default Vue.extend({
  data() {
    return {
    };
  },
  props: {
    row: ReportRow,
  },
  watch: {
    'row.filtered'(val) {
      if (val) {
        this.row.selected = true;
      }
    },
    'row.selected'(val) {
      if (!val) {
        this.row.filtered = false;
      }
    },
  },
  components: {
    CheckBox,
    ReportFilterTypeDropdown,
    LabeledInput,
    DropDown,
  },
});
</script>
