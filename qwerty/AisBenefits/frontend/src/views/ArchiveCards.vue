<template>
  <div class="page_layout home_layout">
    <div class="search_bar">
      <div class="search-icon">
        <i class="fa fa-search"></i>
      </div>
      <LabeledInput placeholder="Поиск" v-model="searchTerm"></LabeledInput>
      <!-- <CommonButton noBackground html="<i class='fa fa-ellipsis-v'></i>"></CommonButton> -->
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
          :snils="card.snils"
          :surName="card.surName"
          :firstName="card.firstName"
          :secondName="card.secondName"
          :birthDate="card.birthDate"
          :address="card.address"
          :employeeType="card.employType"
          :payoutType="card.payoutType"
          :variant="card.extraPayVariant"
          @click="scrolled = false"
        ></CardPreview>
      </Scrollable>
      <Paginator class="shadow_medium" :page="currentPage" :count="pageCount" @change="turnPage"></Paginator>
    </div>
    <CardView
      v-if="isCardSelected"
      @tab_change="tabChanged"
      @resumeClicked="showSolutionPopup = true"
      isArchived
    ></CardView>
    <div class="command_menu">
      <!-- <PlusButton v-if="!isCardSelected" @click="createCard"></PlusButton> -->
      <CrossButton v-if="isCardSelected" @click="closeCard"></CrossButton>
      <!-- <EditButton v-if="isCardSelected" @click="editCard"></EditButton> -->
      <PrintButton v-if="isCardSelected && tab === 'main_info'" @click="printCard"></PrintButton>
      <PrintButton v-if="isCardSelected && tab === 'payout'" @click="printPopup.show = true"></PrintButton>
    </div>
    <Popup class="print_popup" v-show="printPopup.show" @close="printPopup.show = false">
      <!-- <div> -->
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
        <h5>Начало периода</h5>
        <MonthDropDown v-model="printPopup.fromM"></MonthDropDown>
        <LabeledInput type="number" max="9999" v-model="printPopup.fromY"></LabeledInput>
        <div class="popup_br"></div>
        <h5>Конец периода</h5>
        <MonthDropDown v-model="printPopup.toM"></MonthDropDown>
        <LabeledInput type="number" max="9999" v-model="printPopup.toY"></LabeledInput>
      </template>
      <div class="popup_br"></div>
      <CommonButton text="Скачать" @click="printPayout"></CommonButton>
      <CommonButton text="Отмена" @click="printPopup.show = false"></CommonButton>
      <!-- </div> -->
    </Popup>
    <Popup
      :shown="showSolutionPopup"
      :title="solutionPopupTitle"
      @close="showSolutionPopup = false"
    >
      <h5 class="width-140 margin-left-50">Назначение</h5>
      <LabeledInput
        class="width-240"
        pattern="date"
        placeholder="дд.мм.гггг"
        v-model="newSolution.destination"
      ></LabeledInput>
      <div class="popup_br"></div>
      <h5 class="width-140 margin-left-50">Исполнение</h5>
      <LabeledInput
        class="width-240"
        pattern="date"
        placeholder="дд.мм.гггг"
        v-model="newSolution.execution"
      ></LabeledInput>
      <div class="popup_br"></div>
      <h5 class="width-140 margin-left-50">Комментарий</h5>
      <LabeledInput class="width-240" v-model="newSolution.comment" placeholder="комментарий"></LabeledInput>
      <div class="popup_br"></div>
      <CommonButton
        @click="sendSolution"
        :disabled="!newSolution.destination || !newSolution.execution"
        :text="'Отправить'"
      ></CommonButton>
    </Popup>
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
  align-items: baseline;
  width: calc(100% - 36px);
}
.card_layout.cardSelected .scrollable-content {
  width: calc(100% - 48px);
}
.card_layout.cardSelected .dropdown .scrollable-content {
  width: 64px;
}

.print_popup h5 {
  width: 144px;
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
  flex-basis: calc(67% - 88px);
  height: calc(100% - 24px);
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
import CommonButton from '@/components/CommonButton.vue';
import CrossButton from '@/components/Common/Buttons/CrossButton.vue';
import EditButton from '@/components/Common/Buttons/EditButton.vue';
import PlusButton from '@/components/Common/Buttons/PlusButton.vue';
import PrintButton from '@/components/Common/Buttons/PrintButton.vue';
import Paginator from '@/components/Common/Paginator.vue';
import MonthDropDown from '@/components/Common/MonthDropDown.vue';
import Scrollable from '@/components/Scrollable.vue';
import Popup from '@/components/Popup.vue';
import LabeledInput from '@/components/LabeledInput.vue';
import RequestSender from '@/requestSender';
import axios, { AxiosError } from 'axios';
import { formatDate } from '@/models/utils/stringBuilder';

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
      currentTab: '',
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
      showSolutionPopup: false,
      newSolution: {
        destination: formatDate(new Date().toISOString()),
        execution: formatDate(new Date().toISOString()),
        comment: '',
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
    сardList(): boolean {
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
      this.requestSender.loadCard(this.cardId);
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
      let path = '/archive/cards?';
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
      const router = this.$router;
      setTimeout(() => {
        this.actualizeRoute();
      }, 0);
    },
    search() {
      this.requestSender.searchArchiveCards(this.searchTerm, this.currentPage)
        .then((resp: any) => {
          this.pageCount = resp.data.pagesCount;
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
    printPayout() {
      this.requestSender.printCertificateOfPensionSupplement(
        this.selectedCardId,
        `${this.selectedCard.surName} ${this.selectedCard.firstName} ${this.selectedCard.secondName}`,
        `01.${this.printPopup.fromM}.${this.printPopup.fromY}`,
        `01.${this.printPopup.toM}.${this.printPopup.toY}`,
      ).then(() => {
        this.printPopup.show = false;
      });
    },
    sendSolution() {
      axios.post('/api/solutions/resume', {
        personInfoRootId: this.selectedCardId,
        destination: this.newSolution.destination,
        execution: this.newSolution.execution,
        comment: this.newSolution.comment,
      }).then(() => {
        this.requestSender.loadSolutions(this.cardId);

        this.showSolutionPopup = false;

        this.newSolution = {
          destination: formatDate(new Date().toISOString()),
          execution: '',
          comment: '',
        };
      });
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
    EditButton,
    PlusButton,
    PrintButton,
  },
});
</script>
