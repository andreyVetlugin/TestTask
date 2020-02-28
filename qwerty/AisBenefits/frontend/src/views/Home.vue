<template>
  <div class="page_layout home_layout">
    <div class="search_bar">
      <div class="search-icon">
        <i class="fa fa-search"></i>
      </div>
      <LabeledInput placeholder="Поиск" v-model="searchTerm"></LabeledInput>
    </div>
    <div class="card_layout" :class="{cardSelected : isCardSelected}">
      <CardPreview
        @mousewheel="scrolled = true"
        class="shadow_medium"
        v-if="isCardSelected"
        :id="selectedCard.id"
        :number="selectedCard.number"
        :snils="selectedCard.snils"
        :surName="selectedCard.surName"
        :firstName="selectedCard.firstName"
        :secondName="selectedCard.secondName"
        :birthDate="selectedCard.birthDate"
        :address="selectedCard.address"
        :class="{ scrolled: scrolled }"
        @click="scrolled = false"
      ></CardPreview>
      <Scrollable @mousewheel="scrolled = true">
        <CardPreview
          class="shadow_medium"
          v-for="card in сardList"
          :key="card.id"
          v-show="card.id !== selectedCardId"
          :id="card.id"
          :number="card.number"
          :surName="card.surName"
          :firstName="card.firstName"
          :secondName="card.secondName"
          :birthDate="card.birthDate"
          :address="card.address"
          :employeeType="card.employType"
          :payoutType="card.payoutType"
          :variant="card.extraPayVariant"
          :stageApproved="card.approved"
          :paused="card.paused"
          @click="scrolled = false"
        ></CardPreview>
      </Scrollable>
      <Paginator class="shadow_medium" :page="currentPage" :count="pageCount" @change="turnPage"></Paginator>
    </div>
    <CardView v-if="isCardSelected" :tab="currentTab" @tab_change="tabChanged"></CardView>
    <div class="command_menu">
      <PlusButton v-if="!isCardSelected" @click="createCard"></PlusButton>
      <CrossButton v-if="isCardSelected" @click="closeCard"></CrossButton>
      <EditButton v-if="isCardSelected" @click="editCard"></EditButton>
      <PrintButton v-if="isCardSelected && currentTab === 'main_info'" @click="printCard"></PrintButton>
      <PrintButton v-if="isCardSelected && currentTab === 'work_info'" @click="printWorkInfo"></PrintButton>
      <PrintButton v-if="isCardSelected && currentTab === 'payout'" @click="printPopup.show = true"></PrintButton>
      <SymbolCButton v-if="isCardSelected && currentTab === 'main_info'" @click="sendSnilsRequest"></SymbolCButton>
      <PrintButton
        v-if="isCardSelected && currentTab === 'solution'"
        @click="showSolutionPrintPopup = true"
      ></PrintButton>
    </div>
    <Popup class="print_popup" v-show="printPopup.show" @close="printPopup.show = false">
      <h2
        class="stage-approvement"
        :class="{ selected: !printPopup.period }"
        @click="printPopup.period = false"
      >
        <i class="fa fa-check"></i>всё время
      </h2>
      <h2
        class="stage-approvement"
        :class="{ selected: printPopup.period }"
        @click="printPopup.period = true"
      >
        <i class="fa fa-check"></i>период
      </h2>
      <div class="popup_br"></div>
      <template v-if="printPopup.period">
        <MonthDropDown v-model="printPopup.fromM"></MonthDropDown>
        <LabeledInput type="number" max="9999" v-model="printPopup.fromY"></LabeledInput>
        <div class="popup_br"></div>
        <MonthDropDown v-model="printPopup.toM"></MonthDropDown>
        <LabeledInput type="number" max="9999" v-model="printPopup.toY"></LabeledInput>
      </template>
      <div class="popup_br"></div>
      <CommonButton text="Скачать" @click="printPayout"></CommonButton>
      <CommonButton text="Отмена" @click="printPopup.show = false"></CommonButton>
    </Popup>

    <SolutionPrintPopup :shown="showSolutionPrintPopup" @close="showSolutionPrintPopup = false"></SolutionPrintPopup>

    <RequestResultPopup @close="requestResult.showPopup = false" :requestResult="requestResult"></RequestResultPopup>
  </div>
</template>

<style>
.home_layout {
  height: calc(100% - 64px);
}
.card_layout {
  position: relative;
  box-sizing: border-box;
}
.home_layout .paginator {
  position: absolute;
  /* some magicka */
  bottom: 0;
  left: calc(50% - 178px);
}
.card_layout .scrollable-container {
  flex: 1 1 100%;
  height: 100%;
  width: 100%;
}
.card_layout .scrollable-content {
  display: inline-flex;
  flex-wrap: wrap;
  justify-content: space-between;
  align-content: flex-start;
  /* align-items: baseline; */
  width: calc(100% - 36px);
}
.card_layout.cardSelected .scrollable-content {
  width: calc(100% - 48px);
}
.card_layout.cardSelected .dropdown .scrollable-content {
  width: 64px;
}

.print_popup .dropdown,
.print_popup .common-button:not(.no-text),
.print_popup .labeled-input {
  width: 160px;
  margin-left: 16px;
  margin-right: 16px;
}
.print_popup .stage-approvement {
  display: inline-block;
  width: 240px;
}
.print_popup .stage-approvement {
  flex: 1 1 50%;
  text-align: center;
}
.print_popup .stage-approvement .fa {
  color: transparent;
  font-size: 10px;
  padding: 4px;
  margin: 2px 8px;
  height: 18px;
  width: 18px;
  border-radius: 50%;
  background-color: #503f96;
  box-sizing: border-box;
}
.print_popup .stage-approvement:hover .fa {
  background-color: #8a7ec0;
}
.print_popup .stage-approvement:active .fa {
  background-color: #6253a2;
  color: #ffffff;
}
.print_popup .stage-approvement.selected .fa {
  color: #ffffff;
}

.search_bar {
  margin: 16px;
  height: 32px;
  display: flex;
  align-items: center;
  width: 100%;
  flex: 1 1 100%;
}
.search_bar > * {
  height: 32px;
}
.search_bar .labeled-input {
  width: calc(100% - 120px);
  margin-right: 16px;
}
.search_bar .labeled-input .text-input {
  height: 32px;
  padding: 0;
  border: none;
}
.search_bar .common-button {
  padding: 0;
}
.search-icon {
  display: inline-block;
  color: #9084c3;
  text-align: center;
  padding: 8px;
  width: 32px;
  box-sizing: border-box;
}
.search-icon .fa {
  line-height: 0;
  font-size: 16px;
}
</style>

<style scoped>
.home_layout {
  margin: 0;
  flex-wrap: wrap;
  height: calc(100% - 64px);
}

.card_layout {
  display: flex;
  flex-wrap: wrap;
  flex: 0 0;
  flex-basis: calc(100% - 112px);
  justify-content: center;
  align-content: flex-start;
  align-items: baseline;
  margin: 0 0 0 16px;
  height: 100%;
  position: relative;
}
.card-preview {
  flex: 0 0;
  flex-basis: calc(50% - 12px);
  -ms-flex-preferred-size: auto;
  width: calc(50% - 12px);
  margin-bottom: 24px;
  transition: flex 0.3s ease-in-out, height 0.3s ease-in-out;
}

.card_layout.cardSelected {
  flex: 0 0;
  flex-basis: calc(33% - 24px);
  flex-direction: column;
  justify-content: flex-start;
  flex-wrap: nowrap;
  margin-left: 0;
  position: relative;
  align-content: center;
  align-items: center;
}
.card_layout.cardSelected .card-preview {
  width: calc(100% - 16px);
  margin: 0 0 8px 16px;
  flex: 0 0 auto;
}
.card_layout.cardSelected .card-preview.active {
  width: calc(100% - 32px);
  margin: 0 16px 8px;
}
.card_layout.cardSelected .card-preview.scrolled {
  flex: 0 0 88px;
  height: 108px;
}
.card_layout.cardSelected .card-preview.scrolled :not(h1):not(h2) {
  display: none;
}

.card-view {
  flex: 0 0;
  flex-basis: calc(67% - 72px);
  height: calc(100% - 16px);
}

@media (max-width: 1024px) {
  .card_layout .card-preview {
    flex: 0 0;
    flex-basis: calc(100% - 24px);
  }
}
</style>

<script lang="ts">
import moment from 'moment';
import { Component, Vue } from 'vue-property-decorator';
import CardPreview from '@/components/CardPreview.vue';
import CardView from '@/components/CardView/CardView.vue';
import SolutionPrintPopup from '@/components/CardView/Popups/SolutionPrintPopup.vue';
import CommonButton from '@/components/CommonButton.vue';
import CrossButton from '@/components/Common/Buttons/CrossButton.vue';
import SymbolCButton from '@/components/Common/Buttons/SymbolCButton.vue';
import EditButton from '@/components/Common/Buttons/EditButton.vue';
import PlusButton from '@/components/Common/Buttons/PlusButton.vue';
import PrintButton from '@/components/Common/Buttons/PrintButton.vue';
import Paginator from '@/components/Common/Paginator.vue';
import MonthDropDown from '@/components/Common/MonthDropDown.vue';
import Scrollable from '@/components/Scrollable.vue';
import Popup from '@/components/Popup.vue';
import LabeledInput from '@/components/LabeledInput.vue';
import RequestSender from '@/requestSender';
import RequestResultPopup from '@/components/Common/Popups/RequestResultPopup.vue';
import { formatDate } from '@/models/utils/stringBuilder';
import { AxiosError } from 'axios';

export default Vue.extend({
  props: {
    cardId: String,
    term: String,
    tab: String,
    page: Number,
  },
  data() {
    return {
      scrolled: false,
      currentTab: 'main_info',
      currentPage: 1,
      pageCount: 0,
      changed: false,
      searchTerm: '',
      printPopup: {
        show: false,
        period: false,
        fromM: '',
        fromY: '',
        toM: '',
        toY: '',
      },
      showSolutionPrintPopup: false,
      requestResult: {
        success: true,
        message: '',
        showPopup: false,
      },
    };
  },
  computed: {
    requestSender(): RequestSender {
      return this.$store.getters.requestSender;
    },
    isCardSelected(): boolean {
      return this.$store.getters.isCardSelected;
    },
    selectedCardId(): string {
      return this.$store.getters.selectedCardId;
    },
    selectedCard(): any {
      return this.$store.getters.selectedCard;
    },
    сardList(): object[] {
      return this.$store.getters.cardList;
    },
  },
  mounted() {
    this.searchTerm = this.term;
    setTimeout(() => this.changed = false, 0);
    this.currentTab = this.tab || 'main_info';
    this.currentPage = this.page || 1;
    this.search();
    this.$store.commit('setSelectedCardId', this.cardId);
    if (this.cardId) {
      this.requestSender.loadCard(this.cardId)
        .catch((resp: AxiosError) => {
          this.requestResult.success = false;
          this.requestResult.message = 'Ошибка получения данных карточки';
          this.requestResult.showPopup = true;
        });
      this.requestSender.getMinExtraPay();
    }
    const today = moment();
    const month = today.format('MM');
    const year = moment().year().toString();
    this.printPopup.fromM = month;
    this.printPopup.toM = month;
    this.printPopup.fromY = year;
    this.printPopup.toY = year;
  },
  watch: {
    searchTerm: 'searchAsync',
  },
  methods: {
    actualizeRoute() {
      let path = '/cards?';
      if (this.selectedCardId) {
        path += 'cardId=' + (this.selectedCardId || '') + '&';
      }
      path += 'tab=' + (this.currentTab || 'main_info') +
        '&term=' + (this.searchTerm || '') +
        '&page=' + (this.currentPage || 1);
      this.$router.push(path);
    },
    createCard() {
      setTimeout(() => {
        this.$router.push('/create');
      }, 0);
    },
    editCard() {
      setTimeout(() => {
        this.$router.push('/edit?cardId=' + (this.selectedCardId || '') + '&tab=' + this.currentTab);
      }, 0);
    },
    tabChanged(value: string) {
      this.currentTab = value;
      this.actualizeRoute();
    },
    closeCard(): void {
      const empty = '';
      this.$store.commit('setSelectedCardId', empty);
      setTimeout(() => {
        this.actualizeRoute();
      }, 0);
    },
    search() {
      this.requestSender.searchCards(this.searchTerm, this.currentPage)
        .then((resp: any) => {
          this.pageCount = resp.data.pagesCount;
        })
        .catch((resp: AxiosError) => {
          this.requestResult.success = false;
          this.requestResult.message = 'Ошибка получения списка карточек';
          this.requestResult.showPopup = true;
        });
      this.actualizeRoute();
    },
    searchAsync() {
      this.changed = true;
      setTimeout(() => {
        if (this.changed) {
          this.turnPage(1);
          this.changed = false;
        }
      }, 1000);

    },
    turnPage(page: number) {
      this.currentPage = page;
      this.search();
      this.$children.forEach((el: any) => el.hasOwnProperty('contentTop') && (el.contentTop = 0));
    },
    printCard() {
      this.requestSender.printPersonInfo(
        this.selectedCardId,
        `${this.selectedCard.surName} ${this.selectedCard.firstName} ${this.selectedCard.secondName}`,
        formatDate(new Date().toISOString()),
      );
    },
    printWorkInfo() {
      this.requestSender.printWorkInfo(
        this.selectedCardId,
        `${this.selectedCard.surName} ${this.selectedCard.firstName} ${this.selectedCard.secondName}`,
        formatDate(new Date().toISOString()),
      );
    },
    printPayout() {
      const fromM = this.printPopup.period ? this.printPopup.fromM : '01';
      const toM = this.printPopup.period ? this.printPopup.toM : '12';
      const fromY = this.printPopup.period ? this.printPopup.fromY : '1970';
      const toY = this.printPopup.period ? this.printPopup.toY : '9999';
      this.requestSender.printCertificateOfPensionSupplement(
        this.selectedCardId,
        `${this.selectedCard.surName} ${this.selectedCard.firstName} ${this.selectedCard.secondName}`,
        `01.${fromM}.${fromY}`,
        `01.${toM}.${toY}`,
      ).then(() => {
        this.printPopup.show = false;
      });
    },
    sendSnilsRequest() {
      this.requestSender.sendSnilsRequest(this.selectedCardId);
    },
  },
  components: {
    LabeledInput,
    Popup,
    Scrollable,
    Paginator,
    MonthDropDown,
    CardPreview,
    CardView,
    CommonButton,
    CrossButton,
    SymbolCButton,
    EditButton,
    PlusButton,
    PrintButton,
    RequestResultPopup,
    SolutionPrintPopup,
  },
});
</script>
