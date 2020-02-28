<template>
  <div class="tab-content solution_tab">
    <CommonButton
      style="margin-top: 16px;"
      v-if="isArchived"
      @click="$emit('resumeClicked')"
      :text="'Возобновить'"
    ></CommonButton>
    <div class="solution-row header-row">
      <h5>Назначение</h5>
      <h5>Исполнение</h5>
      <h5>Вид решения</h5>
      <h5 class="align-right">Доплата</h5>
      <h5 class="align-right">Пенсия</h5>
      <h5 class="align-right">% МДС</h5>
      <h5 class="flex_60">% ДС</h5>
      <h5 class="align-right">МДС</h5>
      <CommonButton noBackground></CommonButton>
    </div>
    <Scrollable hideScroll>
      <div class="solution-row" v-for="solution in card.solutions" :key="solution.id">
        <h4>{{solution.destination}}</h4>
        <h4>{{solution.execution}}</h4>
        <h4>{{solution.solutionTypeStr}}</h4>
        <h3>
          {{solution.totalExtraPay}}
          <span class="currency">&#8381;</span>
        </h3>
        <h3>
          {{solution.totalPension}}
          <span class="currency">&#8381;</span>
        </h3>
        <h3>
          {{solution.mds}}
          <span class="currency">&#8381;</span>
        </h3>
        <h4 class="flex_60">{{solution.dSperc}}</h4>
        <h3>
          {{solution.ds}}
          <span class="currency">&#8381;</span>
        </h3>
        <PrintButton noBackground @click="printSolution(solution)"></PrintButton>
        <div class="solution-row_comment" v-if="solution.comment">
          <h5>Комментарий:</h5>
          <h4>{{solution.comment}}</h4>
        </div>
      </div>
    </Scrollable>
  </div>
</template>

<style>
.tab-content.solution_tab {
  justify-content: flex-start;
}
.tab-content.solution_tab .scrollable-container {
  /* width: 100%; */
  margin-right: -36px;
  flex: 0 0;
  flex-basis: calc(100% - 130px);
}
.tab-content.solution_tab .scrollable-content {
  /* width: 100%; */
  margin-right: -36px;
  flex: 0 0;
  flex-basis: calc(100% - 130px);
  width: calc(100% - 34px);
}
.solution-row {
  flex: 0 0 100%;
  display: flex;
  flex-wrap: wrap;
  justify-content: space-between;
  align-content: center;
  align-items: center;
  border-bottom: 1px solid #534590;
  padding: 12px 0;
  margin: 0;
}
.solution-row.header-row {
  flex: 0 0 auto;
  width: 100%;
}
.solution-row.header-row .common-button {
  height: 0;
  padding: 0;
  border: none;
}
.solution-row h5,
.solution-row h4,
.solution-row h3 {
  flex: 0 0;
  flex-basis: calc((100% - 124px) / 7);
  width: calc((100% - 124px) / 7);
}
.solution-row h3 {
  text-align: right;
  padding-right: 8px;
  box-sizing: border-box;
  white-space: nowrap;
}
.solution-row .currency {
  color: black;
}
.solution-row .flex_60 {
  flex: 0 0 60px;
  text-align: center;
}
.solution-row .align-right {
  text-align: right;
}
.solution-row .solution-row_comment {
  flex: 0 0;
  flex-basis: calc(100% - 64px);
  display: flex;
  justify-content: space-between;
}
.solution-row .solution-row_comment h5 {
  flex: 1 1 20%;
}
.solution-row .solution-row_comment h4 {
  flex: 1 1 80%;
}

@media (max-width: 1440px) {
  .solution-row h5,
  .solution-row h4,
  .solution-row h3 {
    font-size: 14px;
  }
}
</style>

<script lang="ts">
import Vue from 'vue';
import Scrollable from '@/components/Scrollable.vue';
import CommonButton from '@/components/CommonButton.vue';
import PrintButton from '@/components/Common/Buttons/PrintButton.vue';
import RequestSender from '@/requestSender';
import Solution from '@/models/person/solution';
import PersonInfo from '@/models/person/personInfo';
export default Vue.extend({
  props: {
    isArchived: Boolean,
  },
  computed: {
    selectedCardId(): string {
      return this.$store.getters.selectedCardId;
    },
    card(): PersonInfo {
      return this.$store.getters.selectedCard;
    },
    requestSender(): RequestSender {
      return this.$store.getters.requestSender;
    },
  },
  methods: {
    printSolution(solution: Solution) {
      const fio = [this.card.surName];
      if (this.card.firstName) {
        fio.push(this.card.firstName[0] + '.');
      }
      if (this.card.secondName) {
        fio.push(this.card.secondName[0] + '.');
      }
      const filename = `Решение от ${solution.destination} для ${fio.join(' ')}`;
      this.requestSender.printSolution(solution.id, filename);
    },
  },
  components: {
    Scrollable,
    CommonButton,
    PrintButton,
  },
});
</script>
