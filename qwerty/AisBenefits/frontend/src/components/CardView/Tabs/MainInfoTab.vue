<template>
  <div class="tab-content main-info_tab">
    <Scrollable>
      <div class="main-info_row" v-show="card.number">
        <h5>Регистрационный номер</h5>
        <h4 v-text="card.number"></h4>
      </div>
      <div class="main-info_row" v-show="card.registrationDate">
        <h5>Дата регистрации</h5>
        <h4 v-text="card.registrationDate"></h4>
      </div>
      <div class="main-info_row" v-show="card.employeeType">
        <h5>Тип получателя</h5>
        <h4 v-text="card.employeeType"></h4>
      </div>
      <div class="main-info_row" v-show="card.payoutType">
        <h5>Тип выплаты</h5>
        <h4 v-text="card.payoutType"></h4>
      </div>
      <div class="main-info_row" v-show="card.district">
        <h5>Район получения пенсии</h5>
        <h4 v-text="card.district"></h4>
      </div>
      <div class="main-info_row" v-show="card.pensionCaseNumber">
        <h5>Номер пенсионного дела</h5>
        <h4 v-text="card.pensionCaseNumber"></h4>
      </div>
      <!-- 10 цифр -->
      <div class="main-info_row" v-show="card.pensionType">
        <h5>Тип пенсии</h5>
        <h4 v-text="card.pensionType"></h4>
      </div>
      <div
        class="main-info_row"
        v-show="card.pensionTypeId === 'd666a556-46f1-4b82-a242-ba9c36a0cae0'"
      >
        <h5>Срок выплаты по..</h5>
        <h4 v-text="formatDate(card.pensionEndDate)"></h4>
      </div>
      <!-- только по инвалидности!!! -->
      <div class="main-info_row" v-show="card.additionalPension">
        <h5>Дополнительная пенсия</h5>
        <h4 v-text="card.additionalPension"></h4>
      </div>
      <div class="main-info_row" v-show="card.surName">
        <h5>Фамилия</h5>
        <h4 v-text="card.surName"></h4>
      </div>
      <div class="main-info_row" v-show="card.firstName">
        <h5>Имя</h5>
        <h4 v-text="card.firstName"></h4>
      </div>
      <div class="main-info_row" v-show="card.secondName">
        <h5>Отчество</h5>
        <h4 v-text="card.secondName"></h4>
      </div>
      <div class="main-info_row" v-show="card.sex">
        <h5>Пол</h5>
        <h4 v-text="card.sex"></h4>
      </div>
      <div class="main-info_row" v-show="card.birthDate">
        <h5>Дата рождения</h5>
        <h4 v-text="card.birthDate"></h4>
      </div>
      <div class="main-info_row" v-show="card.birthPlace">
        <h5>Место рождения</h5>
        <h4 v-text="card.birthPlace"></h4>
      </div>
      <div class="main-info_row" v-show="card.snils">
        <h5>СНИЛС</h5>
        <h4 v-text="card.snils"></h4>
      </div>
      <div class="main-info_row" v-show="card.documentType">
        <h5>Тип документа</h5>
        <h4 v-text="card.documentType"></h4>
      </div>
      <div class="main-info_row" v-show="card.documentSeria">
        <h5>Серия документа</h5>
        <h4 v-text="card.documentSeria"></h4>
      </div>
      <div class="main-info_row" v-show="card.documentNumber">
        <h5>Номер документа</h5>
        <h4 v-text="card.documentNumber"></h4>
      </div>
      <div class="main-info_row" v-show="card.documentIssuer">
        <h5>Кем выдан</h5>
        <h4 v-text="card.documentIssuer"></h4>
      </div>
      <div class="main-info_row" v-show="card.documentIssueDate">
        <h5>Когда выдан</h5>
        <h4 v-text="card.documentIssueDate"></h4>
      </div>
      <div class="main-info_row" v-show="card.address">
        <h5>Адрес</h5>
        <h4 v-text="card.address"></h4>
      </div>
      <div class="main-info_row" v-show="card.phone">
        <h5>Телефон</h5>
        <h4 v-text="card.phone"></h4>
      </div>
    </Scrollable>
  </div>
</template>

<style>
.tab-content.main-info_tab {
  justify-content: flex-start;
}
.tab-content h1 {
  margin: 48px 0 40px;
}
.card-view .main-info_tab .scrollable-container {
  flex-basis: calc(100% - 96px);
  margin: 48px 0;
}

.main-info_row {
  margin: 16px 0 8px;
  width: 100%;
  flex: 0 0 100%;
  display: flex;
}
.main-info_row:first-child {
  margin: 0 0 8px;
  width: 100%;
  flex: 0 0 100%;
  display: flex;
}
.main-info_row h5 {
  margin: 0;
  flex: 0 0;
  flex-basis: 33%;
  margin-right: 24px;
}
.main-info_row h4 {
  margin: 0;
  flex: 0 0;
  flex-basis: calc(67% - 24px);
}
</style>


<script lang="ts">
import Vue from 'vue';
import Scrollable from '@/components/Scrollable.vue';
import { formatDate } from '@/models/utils/stringBuilder';
export default Vue.extend({
  computed: {
    selectedCardId(): string {
      return this.$store.getters.selectedCardId;
    },
    card(): object {
      return this.$store.getters.selectedCard;
    },
  },
  methods: {
    formatDate(date: string): string {
      return formatDate(date);
    },
  },
  components: {
    Scrollable,
  },
});
</script>
