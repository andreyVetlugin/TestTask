<template>
  <div class="tab-content work_info">
    <h3
      class="stage-approvement"
      v-show="card.works && card.works.length"
      v-text="card.worksApproved ? 'Стаж подтвержден' : 'Стаж не подтвержден'"
    ></h3>
    <Scrollable>
      <div class="work-info_row" v-for="work in card.works" :key="work.id">
        <h2
          v-text="work.startDate + ' — ' + (work.endDate || 'настоящее время')"
        ></h2>
        <h5>Стаж</h5>
        <h4 v-text="work.age"></h4>
        <h5>Организация</h5>
        <h4 v-text="work.organizationName"></h4>
        <h5>Должность</h5>
        <h4 v-text="work.function"></h4>
      </div>
    </Scrollable>
    <div class="work-info_total-row">
      <LabeledBlock lined>
        <template slot="title">
          <h2>Даты</h2>
        </template>
        <template slot="content">
          <div v-if="workEndDate">
            <template v-show="workEndDate">
              <h5>Увольнения</h5>
              <h4>{{workEndDate}}</h4>
            </template>
          </div>
          <div>
            <template v-show="card.docsDestinationDate">
              <h5>Назначения</h5>
              <h4>{{card.docsDestinationDate}}</h4>
            </template>
          </div>
          <div>
            <template v-show="card.docsSubmitDate">
              <h5>Подачи документов</h5>
              <h4>{{card.docsSubmitDate}}</h4>
            </template>
          </div>
        </template>
      </LabeledBlock>
      <h2>Общий стаж</h2>
      <h3>{{card.worksAge}}</h3>
    </div>
  </div>
</template>

<style>
.card-view .tab-content.work_info .scrollable-container {
  flex-basis: calc(100% - 310px);
}
.work-info_row {
  padding: 40px 0 24px;
  flex: 0 0 100%;
  display: flex;
  justify-content: space-between;
  flex-wrap: wrap;
  border-bottom: 1px solid #534590;
}
.work-info_row h2 {
  margin: 0 0 24px;
  flex: 0 0 100%;
}
.work-info_row h5 {
  flex: 0 0 33%;
}
.work-info_row h4 {
  flex: 0 0;
  flex-basis: calc(67% - 24px);
}

.work-info_total-row {
  padding: 24px 0 48px;
  width: 100%;
  display: flex;
  justify-content: flex-start;
  align-items: center;
  flex-wrap: wrap;
}
.work-info_total-row .labeled_block {
  display: flex;
  flex-direction: row;
  flex-wrap: wrap;
  justify-content: space-between;
  width: 100%;
}
.work-info_total-row .labeled_block-content {
  display: flex;
  flex-direction: row;
  justify-content: space-between;
  flex-basis: 100%;
}
.work-info_total-row > h2 {
  flex: 0 0 160px;
  margin: 0 0 24px;
}
.work-info_total-row h3 {
  flex: 0 0;
  flex-basis: calc(100% - 184px);
  margin: 0 0 24px;
}
.work-info_total-row .labeled_block-title h2 {
  margin: 0 24px 0 0;
}

.stage-approvement {
  margin: 40px 0 0;
}
</style>


<script lang="ts">
import Vue from 'vue';
import Scrollable from '@/components/Scrollable.vue';
import LabeledBlock from '@/components/Common/LabeledBlock.vue';
import PersonInfo from '@/models/person/personInfo';
export default Vue.extend({
  computed: {
    selectedCardId(): string {
      return this.$store.getters.selectedCardId;
    },
    card(): PersonInfo {
      return this.$store.getters.selectedCard;
    },
    workEndDate(): string {
      return (this.card && this.card.works) ? (this.card.works[0] || {}).endDate : '';
    },
  },
  components: {
    Scrollable,
    LabeledBlock,
  },
});
</script>
