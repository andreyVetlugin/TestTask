<template>
  <div class="card-view shadow_medium">
    <div class="tab-navigation">
      <CommonButton
        :class="{ selected: selectedTab === 'main_info'}"
        @click="changeTab('main_info')"
        text="Карта"
      ></CommonButton>
      <CommonButton
        :class="{ selected: selectedTab === 'work_info'}"
        @click="changeTab('work_info')"
        text="Стаж"
      ></CommonButton>
      <CommonButton
        :class="{ selected: selectedTab === 'extra_pay'}"
        @click="changeTab('extra_pay')"
        text="Расчёт доплат"
      ></CommonButton>
      <CommonButton
        :class="{ selected: selectedTab === 'recount'}"
        @click="changeTab('recount')"
        text="Перерасчёт"
      ></CommonButton>
      <CommonButton
        :class="{ selected: selectedTab === 'solution'}"
        @click="changeTab('solution')"
        text="Решения"
      ></CommonButton>
      <CommonButton
        :class="{ selected: selectedTab === 'payout'}"
        @click="changeTab('payout')"
        text="Выплаты"
      ></CommonButton>
    </div>
    <MainInfoTab v-if="selectedTab === 'main_info'"></MainInfoTab>
    <WorkInfoTab v-if="selectedTab === 'work_info'"></WorkInfoTab>
    <ExtraPayTab v-if="selectedTab === 'extra_pay'"></ExtraPayTab>
    <RecountTab v-if="selectedTab === 'recount'"></RecountTab>
    <SolutionTab v-if="selectedTab === 'solution'" @resumeClicked="$emit('resumeClicked')" :isArchived="isArchived"></SolutionTab>
    <PayoutTab v-if="selectedTab === 'payout'"></PayoutTab>
  </div>
</template>

<style>
.card-view {
  border-radius: 4px;
  background-color: #6254a3;
  box-sizing: border-box;
}
.tab-navigation {
  border-color: #5d4ca3;
  display: flex;
  justify-content: space-between;
  flex-wrap: nowrap;
  width: 100%;
}
.tab-navigation .common-button {
  border-radius: 0;
  flex: 1 1 auto;
  color: #c6bcee;
  font-family: "Rubik-Light";
  padding: 23px 0;
}
.tab-navigation .common-button.selected,
.tab-navigation .common-button:hover,
.tab-navigation .common-button:active {
  color: #ffffff;
}
.tab-navigation .common-button:first-child {
  border-top-left-radius: 4px;
}
.tab-navigation .common-button:last-child {
  border-top-right-radius: 4px;
}

.card-view .scrollable-container {
  width: calc(100% + 34px);
  flex: 0 0;
  flex-basis: calc(100% - 200px);
}

.tab-content {
  height: calc(100% - 64px);
  padding: 0 48px;
  box-sizing: border-box;
  display: flex;
  justify-content: space-between;
  align-content: flex-start;
  align-items: baseline;
  flex-wrap: nowrap;
  flex-direction: column;
}

@media (max-width: 1440px) {
  .tab-content {
    padding: 0 32px;
  }
}
</style>

<script lang="ts">
import Vue from 'vue';
import MainInfoTab from './Tabs/MainInfoTab.vue';
import WorkInfoTab from './Tabs/WorkInfoTab.vue';
import ExtraPayTab from './Tabs/ExtraPayTab.vue';
import RecountTab from './Tabs/RecountTab.vue';
import SolutionTab from './Tabs/SolutionTab.vue';
import PayoutTab from './Tabs/PayoutTab.vue';
import CommonButton from '@/components/CommonButton.vue';
import RequestSender from '@/requestSender';
export default Vue.extend({
  props: {
    tab: String,
    isArchived: Boolean,
  },
  data() {
    return {
      selectedTab: 'main_info',
    };
  },
  mounted() {
    this.selectedTab = this.tab || 'main_info';
    if (this.selectedCardId) {
      this.requestSender.loadCard(this.selectedCardId)
        .then(() => {
          this.requestSender.loadSolutions(this.selectedCardId);
          this.requestSender.getPersonBankCard(this.selectedCardId);
          this.requestSender.getMinExtraPay();
        });
    }
  },
  computed: {
    requestSender(): RequestSender {
      return this.$store.getters.requestSender;
    },
    selectedCardId(): string {
      return this.$store.getters.selectedCardId;
    },
    card(): object {
      return this.$store.getters.selectedCard;
    },
  },
  watch: {
    selectedCardId(cardId) {
      if (cardId) {
        this.requestSender.loadCard(this.selectedCardId)
          .then(() => {
            this.requestSender.loadSolutions(this.selectedCardId);
            this.requestSender.getPersonBankCard(this.selectedCardId);
          });
      }
    },
    tab(val, old) {
      this.selectedTab = val || old || 'main_info';
    },
  },
  methods: {
    changeTab(tab: string) {
      if (!tab) {
        return;
      }

      this.selectedTab = tab;
      this.$emit('tab_change', tab);
    },
  },
  components: {
    MainInfoTab,
    WorkInfoTab,
    ExtraPayTab,
    RecountTab,
    SolutionTab,
    PayoutTab,
    CommonButton,
  },
});
</script>
