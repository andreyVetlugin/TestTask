<template>
  <div class="page_layout reports_layout">
    <div class="report_block_list">
      <div
        v-for="(block, i) in blocks"
        :key="i"
        :class="{ selected: selectedBlock === block.value}"
        @click="selectedBlock = block.value"
        class="report_block shadow_light"
      >
        <h2>{{block.title}}</h2>
      </div>
    </div>
    <div class="report_block_view shadow_medium">
      <CommonButton class="select_button" text="Выбрать всё" @click="setAllActive(true)"></CommonButton>
      <CommonButton class="select_button" text="Снять всё" @click="setAllActive(false)"></CommonButton>
      <Scrollable hideScroll>
        <ReportRow v-for="(row, i) in report[selectedBlock]" :row="row" :key="i"></ReportRow>
      </Scrollable>
    </div>
    <div class="command_menu">
      <SaveButton @click="loadReport"></SaveButton>
    </div>
  </div>
</template>

<style>
.report_block_list {
  display: flex;
  flex-wrap: nowrap;
  align-content: flex-start;
  align-items: baseline;
  overflow-x: auto;
  overflow-y: none;
  flex: 0 0;
  flex-basis: calc(33% - 32px);
  flex-direction: column;
  justify-content: flex-start;
}
.report_block {
  background-color: #3f3176;
  border: 1px solid #493c80;
  border-radius: 4px;
  box-sizing: border-box;
  padding: 16px 24px;
  font-size: 0;
  margin: 0 16px 8px;
  width: calc(100% - 32px);
  flex: 0 0 auto;
}
.report_block.selected {
  background-color: #503f96;
  border: 1px solid #3a3066;
}
.report_block h2 {
  margin: 0;
}

.report_block_view {
  display: flex;
  height: 100%;
  border-radius: 4px;
  background-color: #6254a3;
  box-sizing: border-box;
  flex: 0 0;
  flex-basis: calc(67% - 64px);
  flex-wrap: wrap;
  padding: 24px 48px;
  align-content: flex-start;
}
.report_block_view .scrollable-container {
  flex: 0 0 100%;
  height: calc(100% - 48px);
}
.report_block_view .report_row {
  flex: 0 0 100%;
  height: 64px;
}
.report_block_view .select_button {
  height: 48px;
  padding: 16px;
  width: 160px;
  margin-right: 16px;
}
</style>


<script lang="ts">
import Vue from 'vue';
import ReportRow from '@/components/Reports/ReportRow.vue';
import Scrollable from '@/components/Scrollable.vue';
import SaveButton from '@/components/Common/Buttons/SaveButton.vue';
import CommonButton from '@/components/CommonButton.vue';
import ReportModel from '@/models/reports/reportModel';
import RequestSender from '@/requestSender';
export default Vue.extend({
  data() {
    return {
      report: new ReportModel(),
      selectedBlock: 'PIForm',
      blocks: [
        { value: 'PIForm', title: 'Карта' },
        { value: 'WIForm', title: 'Стаж' },
        { value: 'EPForm', title: 'Расчёты доплат' },
        { value: 'SolForm', title: 'Решения' },
        { value: 'PayoutForm', title: 'Выплаты' },
      ],
    };
  },
  mounted() {
    this.requestSender.loadCardEditData();
    this.requestSender.getAllFunctions();
    this.requestSender.getAllOrganizations();
    this.requestSender.getAllExtraPayVariants();
  },
  computed: {
    requestSender(): RequestSender {
      return this.$store.getters.requestSender;
    },
    cardEditData(): RequestSender {
      return this.$store.getters.cardEditData;
    },
    functionsList(): Array<{}> {
      return this.$store.getters.functionsList.map((el: any) => {
        return {
          value: el.id,
          title: el.name,
        };
      });
    },
    organizationsList(): Array<{}> {
      return this.$store.getters.organizationsList.map((el: any) => {
        return {
          value: el.id,
          title: el.organizationName,
        };
      });
    },
    extraPayVariants(): Array<{}> {
      return this.$store.getters.extraPayVariants
        .map((el: any) => {
          return {
            value: el.id,
            title: el.number.toString(),
          };
        });
    },
  },
  watch: {
    selectedBlock() {
      this.$children.forEach((el: any) => el.hasOwnProperty('contentTop') && (el.contentTop = 0));
    },
    cardEditData(data) {
      this.report.setCardEditData(data);
    },
    functionsList(data) {
      this.report.setFunctions(data);
    },
    organizationsList(data) {
      this.report.setOrganizations(data);
    },
    extraPayVariants(data) {
      this.report.setExtraPayVariants(data);
    },
  },
  methods: {
    setAllActive(val: boolean) {
      this.report.setAll(this.selectedBlock, val);
    },
    loadReport() {
      this.requestSender.buildReport(this.report);
    },
  },
  components: {
    ReportRow,
    Scrollable,
    SaveButton,
    CommonButton,
  },
});
</script>
