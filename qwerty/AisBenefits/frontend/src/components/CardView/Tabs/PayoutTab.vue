<template>
  <div class="tab-content payouts_tab">
    <Scrollable>
      <LabeledBlock v-for="year in yearedPayouts" :key="year.year" lined>
        <template slot="title">
          <h2>{{year.year}}</h2>
        </template>
        <template slot="content">
          <div
            class="payout_block"
            v-for="(payout, i) in year.payouts"
            :key="i"
            :style="{ order: (12 - getMonthNumber(payout.title)) }"
          >
            <h5>
              {{payout.title}}
              <!-- <ExpandableTitle v-if="payout.comment" :title="payout.comment"></ExpandableTitle> -->
            </h5>
            <h3>
              {{payout.value}}
              <span class="currency">&#8381;</span>
            </h3>
            <h4>{{payout.comment}}</h4>
          </div>
        </template>
      </LabeledBlock>
    </Scrollable>
    <div class="bank_card-container">
      <div class="bank_card" v-if="bankCard.type === 0 || bankCard.number">
        <h5>Номер карты Сбербанка</h5>
        <h4>{{bankCard.number || 'не указан'}}</h4>
        <h5>Окончание действия</h5>
        <h4>
          {{bankCard.validThru || 'не указано'}}
          <span
            class="valid_thru error"
            v-if="bankCard.validThru && bankCard.validRemains <= 0"
          >(Карта просрочена)</span>
          <span
            class="valid_thru warning"
            v-if="bankCard.validThru && 0 < bankCard.validRemains && bankCard.validRemains <= 30"
          >(Осталось {{ageToString(0, 0, bankCard.validRemains)}})</span>
        </h4>
      </div>
      <div class="bank_card" v-else>
        <h5>Номер счета ЕМБ-банка</h5>
        <h4>{{bankCard.account || 'не указан'}}</h4>
      </div>
    </div>
  </div>
</template>

<style>
.card-view .tab-content.payouts_tab .scrollable-container {
  flex-basis: calc(100% - 144px);
}

.payout-row {
  width: 100%;
  flex: 0 0 100%;
  padding: 24px 0;
  display: flex;
  justify-content: space-between;
  border-bottom: 1px solid #534590;
}
.align-right {
  text-align: right;
  padding-right: 24px;
  box-sizing: border-box;
}
.align-center {
  text-align: center;
}
.payout-row:first-child {
  margin-top: 24px;
}
.payout-row .flex_80px {
  flex: 0 0 80px;
}
.payout-row .flex_160px {
  flex: 0 0 160px;
}
.payout-row .flex_full {
  flex: 1 1 auto;
  word-break: break-word;
}

.tab-content.payouts_tab h2 {
  margin: 24px;
}
.tab-content.payouts_tab .labeled_block-content {
  display: flex;
  justify-content: flex-start;
  flex-wrap: wrap;
}
.tab-content.payouts_tab .labeled_block-title h2 {
  margin: 16px 16px 16px 0;
}
.payout_block {
  /* flex: 0 0 33.333%; */
  flex: 0 0 100%;
  display: flex;
}
.payout_block h3,
.payout_block h5 {
  flex: 0 0 25%;
}
.payout_block h4 {
  flex: 0 0 50%;
}
.currency {
  color: black;
}

.bank_card-container {
  padding-bottom: 32px;
  width: 100%;
  display: flex;
  flex-wrap: wrap;
}
.bank_card-container .bank_card {
  width: calc(100% - 112px);
  display: flex;
  flex-wrap: wrap;
}
.tab-content.payouts_tab .bank_card-container h2 {
  margin-left: 0;
  flex: 0 0 100%;
}
.bank_card-container h5 {
  flex: 0 0 30%;
}
.bank_card-container h4 {
  flex: 0 0 70%;
}
.valid_thru.error {
  color: red;
}
.valid_thru.warning {
  color: #f1b238;
}
</style>


<script lang="ts">
import Vue from 'vue';
import Scrollable from '@/components/Scrollable.vue';
import ExpandableTitle from '@/components/ExpandableTitle.vue';
import LabeledBlock from '@/components/Common/LabeledBlock.vue';
import PersonInfo from '@/models/person/personInfo';
import Payout from '@/models/person/payout';
import { ageToString } from '@/models/utils/stringBuilder';
export default Vue.extend({
  computed: {
    selectedCardId(): string {
      return this.$store.getters.selectedCardId;
    },
    card(): PersonInfo {
      return this.$store.getters.selectedCard;
    },
    bankCard(): any {
      return this.$store.getters.selectedCard
        ? this.$store.getters.selectedCard.bankCard || {}
        : {};
    },
    yearedPayouts(): any[] {
      const result: any[] = [];
      this.card.payouts
        .sort((a: any, b: any) => {
          return b.compare(a);
        }).forEach((el: Payout) => {
          let year = result.find((elem: any) => elem.year === el.year);
          if (!year) {
            year = {
              year: el.year,
              payouts: [],
            };
            result.push(year);
          }
          year.payouts.push({
            title: el.month,
            value: el.amount,
            comment: el.comment,
          });
        });
      return result;
    },
  },
  methods: {
    ageToString(y: number, m: number, d: number, s?: string) {
      return ageToString(y, m, d, s || ',');
    },
    getMonthNumber(month: string): number {
      switch (month.toLowerCase()) {
        case 'январь': return 1;
        case 'февраль': return 2;
        case 'март': return 3;
        case 'апрель': return 4;
        case 'май': return 5;
        case 'июнь': return 6;
        case 'июль': return 7;
        case 'август': return 8;
        case 'сентябрь': return 9;
        case 'октябрь': return 10;
        case 'ноябрь': return 11;
        case 'декабрь': return 12;
      }
      return 0;
    },
  },
  components: {
    Scrollable,
    ExpandableTitle,
    LabeledBlock,
  },
});
</script>
