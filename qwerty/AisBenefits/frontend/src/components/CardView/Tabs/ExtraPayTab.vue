<template>
  <div class="tab-content extra-pay_tab">
    <Scrollable class="extra-pay_scrollable">
      <LabeledBlock lined>
        <template slot="title">
          <h2>Начисления</h2>
        </template>
        <template slot="content">
          <div class="extra-pay_row">
            <h5>Уральский к-т</h5>
            <h3>
              {{extraPay.uralMultiplier}}
              <span class="currency">&#8381;</span>
            </h3>
          </div>
          <div class="extra-pay_row">
            <h5>Оклад</h5>
            <h3>
              {{extraPay.salary}}
              <span class="currency">&#8381;</span>
            </h3>
          </div>
          <div class="extra-pay_row">
            <h5>Оклад (с ур. коэф.)</h5>
            <h3>
              {{extraPay.salaryMultiplied}}
              <span class="currency">&#8381;</span>
            </h3>
          </div>
          <div class="extra-pay_row">
            <h5>Премия</h5>
            <h3>
              {{extraPay.premium}}
              <span class="currency">&#8381;</span>
            </h3>
            <h3>
              {{calculatePercents(extraPay.premium)}}
              <span class="currency">%</span>
            </h3>
          </div>
          <div class="extra-pay_row">
            <h5>Мат. помощь</h5>
            <h3>
              {{extraPay.materialSupport}}
              <span class="currency">&#8381;</span>
            </h3>
          </div>
          <div class="extra-pay_row">
            <h5>Надбавки</h5>
            <h3>
              {{extraPay.perks}}
              <span class="currency">&#8381;</span>
            </h3>
            <h3>
              {{calculatePercents(extraPay.perks)}}
              <span class="currency">%</span>
            </h3>
          </div>
          <div class="extra-pay_row">
            <h5>Выслуга</h5>
            <h3>
              {{extraPay.vysluga}}
              <span class="currency">&#8381;</span>
            </h3>
            <h3>
              {{calculatePercents(extraPay.vysluga)}}
              <span class="currency">%</span>
            </h3>
          </div>
          <div class="extra-pay_row">
            <h5>Секретность</h5>
            <h3>
              {{extraPay.secrecy}}
              <span class="currency">&#8381;</span>
            </h3>
            <h3>
              {{calculatePercents(extraPay.secrecy)}}
              <span class="currency">%</span>
            </h3>
          </div>
          <div class="extra-pay_row">
            <h5>Классный чин</h5>
            <h3>
              {{extraPay.qualification}}
              <span class="currency">&#8381;</span>
            </h3>
            <h3>
              {{calculatePercents(extraPay.qualification)}}
              <span class="currency">%</span>
            </h3>
          </div>
        </template>
      </LabeledBlock>

      <LabeledBlock lined>
        <template slot="title">
          <h2>МДС</h2>
        </template>
        <template slot="content">
          <div class="extra-pay_row">
            <h5>Денежное содержание</h5>
            <h4>
              {{extraPay.ds}}
              <span class="currency">&#8381;</span>
            </h4>
          </div>
          <div class="extra-pay_row">
            <h5>% Денежного содержания</h5>
            <h4>
              {{extraPay.dsPerc}}
              <span class="currency">%</span>
            </h4>
          </div>
        </template>
      </LabeledBlock>

      <LabeledBlock lined>
        <template slot="title">
          <h2>Основная пенсия</h2>
        </template>
        <template slot="content">
          <div class="extra-pay_row">
            <h5>Пенсия по старости</h5>
            <h3>
              {{extraPay.gosPension}}
              <span class="currency">&#8381;</span>
            </h3>
          </div>
          <div class="extra-pay_row">
            <h5>Доп. пенсия</h5>
            <h3>
              {{extraPay.extraPension}}
              <span class="currency">&#8381;</span>
            </h3>
          </div>
          <div class="extra-pay_row">
            <h5>Мин. доплата</h5>
            <h4>
              {{minExtraPay}}
              <span class="currency">&#8381;</span>
            </h4>
          </div>
          <div class="extra-pay_row full_width">
            <CommonButton text="Запросить из ПФР" @click="syncGosPension"></CommonButton>
          </div>
        </template>
      </LabeledBlock>

      <LabeledBlock lined>
        <template slot="title">
          <h2>Итог</h2>
        </template>
        <template slot="content">
          <div class="extra-pay_row">
            <h5>Госпенсия</h5>
            <h3>
              {{extraPay.totalPension}}
              <span class="currency">&#8381;</span>
            </h3>
          </div>
          <div class="extra-pay_row">
            <h5>% МДС</h5>
            <h3>
              {{extraPay.totalPensionAndExtraPay}}
              <span class="currency">&#8381;</span>
            </h3>
          </div>
          <div class="extra-pay_row">
            <h5>{{totalExtraPayTitle}}</h5>
            <h4>
              {{extraPay.totalExtraPay}}
              <span class="currency">&#8381;</span>
            </h4>
          </div>
        </template>
      </LabeledBlock>
    </Scrollable>

    <Popup class="sync_popup" :shown="showSyncPopup" @close="showSyncPopup = false">
      <template>
        <h2>Запрос выполнен</h2>
        <CommonButton @click="redirectToPfr" text="Перейти в Межвед"></CommonButton>
      </template>
    </Popup>
  </div>
</template>

<style>
.tab-content.extra-pay_tab {
  justify-content: flex-start;
}
.extra-pay_tab .scrollable-container.extra-pay_scrollable {
  flex: 1 1 100%;
  margin: 48px 0 24px;
}
.extra-pay_tab .labeled_block-content {
  padding: 12px 0 0;
}
.extra-pay_row {
  margin: 0 0 24px;
  width: 33%;
  display: inline-flex;
  flex-wrap: wrap;
}
.extra-pay_row.full_width {
  width: 100%;
}
.extra-pay_row h5 {
  margin: 0;
  flex: 0 0 100%;
  margin-right: 24px;
}
.extra-pay_row h3,
.extra-pay_row h4 {
  margin: 0;
  flex: 0 0 auto;
  margin-right: 16px;
  font-size: 14pt;
  line-height: 20pt;
}
.extra-pay_row .currency {
  color: black;
}

.labeled_block-title h2 {
  margin: 16px 16px 16px 0;
  flex: 0 0 auto;
}

.sync_popup .popup-content h2 {
  flex: 0 0 100%;
}
</style>

<script lang="ts">
import Vue from 'vue';
import Scrollable from '@/components/Scrollable.vue';
import Popup from '@/components/Popup.vue';
import CommonButton from '@/components/CommonButton.vue';
import LabeledBlock from '@/components/Common/LabeledBlock.vue';
import ExtraPay from '@/models/person/extraPayInfo';
import { formatMoney } from '@/models/utils/stringBuilder';
import { calculatePercents } from '@/models/utils/calculations';
import RequestSender from '@/requestSender';
import PersonInfo from '@/models/person/personInfo';
export default Vue.extend({
  data() {
    return {
      showSyncPopup: false,
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
    extraPay(): ExtraPay {
      return this.$store.getters.selectedCard.extraPay;
    },
    minExtraPay(): string {
      return this.$store.getters.minExtraPay;
    },
    totalExtraPayTitle(): string {
      return this.card.payoutTypeId === '0235a5e2-fe16-4967-bb8a-dd91e9bb76bd'
      ? 'Пенсия по выслуге лет'
      : 'Доплата';
    },
  },
  methods: {
    calculatePercents(value: string): string {
      return formatMoney(calculatePercents(this.extraPay.salaryMultiplied, value));
    },
    syncGosPension() {
      this.requestSender.syncGosPension(this.selectedCardId)
        .then(() => {
          this.showSyncPopup = true;
        });
    },
    redirectToPfr() {
      this.showSyncPopup = false;
      this.$router.push('/pension_update');
    },
  },
  components: {
    Scrollable,
    CommonButton,
    LabeledBlock,
    Popup,
  },
});
</script>
