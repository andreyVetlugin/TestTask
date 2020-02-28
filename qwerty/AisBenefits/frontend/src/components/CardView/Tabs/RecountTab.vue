<template>
  <div class="tab-content recount_tab">
    <!-- <span class="currency">&#8381;</span> -->
    <div class="input_block">
      <LabeledInput v-model="from" title="Дата изменения" pattern="date"></LabeledInput>
      <LabeledInput v-model="mainPension" title="Пенсия по старости" pattern="money"></LabeledInput>
      <LabeledInput v-model="additionalPension" title="Доп. пенсия" pattern="money"></LabeledInput>
      <CommonButton :disabled="!canRecalculate" text="Пересчитать" @click="recount"></CommonButton>
    </div>
    <Scrollable v-if="showResults">
      <LabeledBlock lined>
        <template slot="title">
          <h2>Причины</h2>
        </template>
        <template slot="content">
          <div class="recount_row">
            <h5>Месяц</h5>
            <h5>Выплачено</h5>
            <h5>С учётом перерасчета</h5>
            <h5>Разница</h5>
          </div>
          <div v-for="(pay, i) in repayed" class="recount_row" :key="'repayed_' + i">
            <h4>{{pay.month}}</h4>
            <h3>
              {{pay.current}}
              <span class="currency">&#8381;</span>
            </h3>
            <h3>
              {{pay.full}}
              <span class="currency">&#8381;</span>
            </h3>
            <h3>
              {{pay.diff}}
              <span class="currency">&#8381;</span>
            </h3>
          </div>
          <div class="recount_row">
            <h4>Итого:</h4>
            <h3>
              {{totalDiff}}
              <span class="currency">&#8381;</span>
            </h3>
          </div>
        </template>
      </LabeledBlock>
      <LabeledBlock lined>
        <template slot="title">
          <h2>Следствия</h2>
        </template>
        <template slot="buttons">
          <CommonButton @click="showRecountPopup = true" text="Изменить"></CommonButton>
        </template>
        <template slot="content">
          <div class="recount_row">
            <h5>Месяц</h5>
            <h5>К выплате</h5>
            <h5>С учётом перерасчета</h5>
            <h5>Разница</h5>
          </div>
          <div v-for="(pay, i) in recounted" class="recount_row" :key="'recounted_' + i">
            <h4>{{pay.month}}</h4>
            <h3>
              {{pay.current}}
              <span class="currency">&#8381;</span>
            </h3>
            <h3>
              {{pay.full}}
              <span class="currency">&#8381;</span>
            </h3>
            <h3>
              {{pay.diff}}
              <span class="currency">&#8381;</span>
            </h3>
          </div>
        </template>
      </LabeledBlock>
    </Scrollable>
    <div class="input_block">
      <CommonButton @click="applyRecount" text="Применить"></CommonButton>
    </div>
    <Popup
      title="Установить месячную выплату"
      :shown="showRecountPopup"
      @close="showRecountPopup = false"
    >
      <template>
        <LabeledInput v-model="monthlyPayout" pattern="money"></LabeledInput>
        <div class="popup_br"></div>
        <h5>Не {{this.totalDiff.startsWith('-') ? 'более' : 'менее'}}: {{newPayout}}</h5>
        <div class="popup_br"></div>
        <CommonButton :disabled="!canRecount" text="Установить" @click="setMonthlyPayout"></CommonButton>
      </template>
    </Popup>
  </div>
</template>

<style>
.card-view .tab-content.recount_tab .scrollable-container {
  /* flex-basis: calc(100% - 144px); */
  flex: 1 1 100%;
}

.recount_tab .input_block {
  display: flex;
  flex: 0 0 auto;
  width: 100%;
  margin: 48px 0 24px;
  flex-direction: row;
  justify-content: space-between;
  align-items: flex-end;
}
.recount_tab .input_block .text-input {
  background-color: #503f96;
}

.recount_tab .labeled_block-title .common-button {
  height: 48px;
  margin: 12px 0 12px 12px;
}

.recount_row {
  width: 100%;
  flex: 0 0 100%;
  display: flex;
  justify-content: space-between;
  border-bottom: 1px solid #534590;
}
.recount_row:last-child {
  border-bottom: none;
}
.recount_row h5,
.recount_row h4,
.recount_row h3 {
  flex: 0 0 25%;
}
.recount_row h4 {
  text-transform: capitalize;
}

.tab-content.recount_tab h2 {
  margin: 24px;
}
.tab-content.recount_tab .labeled_block-content {
  display: flex;
  justify-content: flex-start;
  flex-wrap: wrap;
}
.tab-content.recount_tab .labeled_block-title h2 {
  margin: 16px 16px 16px 0;
}
.currency {
  color: black;
}
</style>


<script lang="ts">
import Vue from 'vue';
import moment from 'moment';
import Scrollable from '@/components/Scrollable.vue';
import ExpandableTitle from '@/components/ExpandableTitle.vue';
import LabeledBlock from '@/components/Common/LabeledBlock.vue';
import LabeledInput from '@/components/LabeledInput.vue';
import CommonButton from '@/components/CommonButton.vue';
import Popup from '@/components/Popup.vue';
import EditButton from '@/components/Common/Buttons/EditButton.vue';
import PersonInfo from '@/models/person/personInfo';
import RecountRow from '@/models/person/recountRow';
import Payout from '@/models/person/payout';
import {
  substractStrings,
  summStrings,
  floatParse,
  calculatePercents,
  calculateSumm,
} from '@/models/utils/calculations';
import { formatDate, formatMoney, ruDateToISO } from '@/models/utils/stringBuilder';
import RequestSender from '@/requestSender';
export default Vue.extend({
  data() {
    return {
      from: formatDate(new Date().toISOString()),
      mainPension: '0.00',
      additionalPension: '0.00',
      showResults: false,
      showRecountPopup: false,
      monthlyPayout: '',
      monthlyDiff: '',

      repayed: [] as RecountRow[],
      totalDiff: '0.00',
      recounted: [] as RecountRow[],
      newPayout: '0.00',
    };
  },
  computed: {
    requestSender(): RequestSender {
      return this.$store.getters.requestSender;
    },
    selectedCardId(): string {
      return this.$store.getters.selectedCardId;
    },
    card(): PersonInfo {
      return this.$store.getters.selectedCard;
    },
    canRecalculate(): boolean {
      return (this.from && this.mainPension && this.additionalPension) ? true : false;
    },
    canRecount(): boolean {
      if (this.monthlyPayout) {
        return this.totalDiff.startsWith('-')
          ? floatParse(substractStrings(this.newPayout, this.monthlyPayout)) > 0
          : floatParse(substractStrings(this.newPayout, this.monthlyPayout)) < 0;
      }
      return false;
    },
  },
  watch: {
    from() {
      this.showResults = false;
    },
    mainPension(val) {
      this.showResults = false;
      this.newPayout = formatMoney(
        substractStrings(this.card.extraPay.totalPensionAndExtraPay,
          summStrings([val, this.additionalPension])),
      );
    },
    additionalPension(val) {
      this.showResults = false;
      this.newPayout = formatMoney(
        substractStrings(this.card.extraPay.totalPensionAndExtraPay,
          summStrings([this.mainPension, val])),
      );
    },
  },
  methods: {
    recount() {
      this.requestSender.personInfoGetRecount({
        personInfoRootId: this.selectedCardId,
        date: this.from,
        gosPension: floatParse(this.mainPension),
        extraPension: floatParse(this.additionalPension),
      }).then((resp) => {
        this.repayed = resp.data.monthElems
          .map((el: any) => {
            return new RecountRow({
              month: el.date,
              current: formatMoney(el.summ),
              full: formatMoney(el.realSumm),
              diff: formatMoney(el.diff),
            });
          });
        this.newPayout = formatMoney(resp.data.newSumm);
        this.totalDiff = formatMoney(resp.data.aggDiff);
        this.showResults = true;
        const perc = this.totalDiff.startsWith('-') ? '80' : '120';
        this.monthlyPayout = formatMoney(calculateSumm(this.newPayout, perc));
        this.setMonthlyPayout();
      });
    },
    setMonthlyPayout() {
      this.monthlyDiff = formatMoney(substractStrings(this.newPayout, this.monthlyPayout));

      const result = [];
      let total = floatParse(this.totalDiff);
      const monthly = floatParse(this.monthlyDiff) * -1;
      const date = moment().locale('ru');
      while (Math.abs(total) > Math.abs(monthly)) {
        result.push(new RecountRow({
          month: date.format('MMMM'),
          current: formatMoney(substractStrings(this.newPayout, this.monthlyDiff)),
          full: this.newPayout,
          diff: formatMoney(monthly),
        }));
        total -= monthly;
        date.add(1, 'months');
      }
      if (total !== 0) {
        result.push(new RecountRow({
          month: date.format('MMMM'),
          current: formatMoney(substractStrings(this.newPayout, formatMoney(total * -1))),
          full: this.newPayout,
          diff: formatMoney(total),
        }));
      }
      this.recounted = result;

      this.showRecountPopup = false;
    },
    applyRecount() {
      this.requestSender.personInfoConfirmRecount({
        personInfoRootId: this.selectedCardId,
        date: this.from,
        gosPension: floatParse(this.mainPension),
        extraPension: floatParse(this.additionalPension),
        summ: floatParse(this.monthlyPayout),
      });
    },
  },
  components: {
    Scrollable,
    ExpandableTitle,
    LabeledBlock,
    LabeledInput,
    Popup,
    CommonButton,
    EditButton,
  },
});
</script>
