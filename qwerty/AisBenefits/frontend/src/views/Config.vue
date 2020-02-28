<template>
  <div class="page_layout config">
    <div class="dictionaries-list">
      <div
        :class="{ selected: selectedDictionary === 'functions'}"
        @click="selectedDictionary = 'functions'"
        class="dictionary shadow_light"
      >
        <h2>Должности</h2>
      </div>
      <div
        :class="{ selected: selectedDictionary === 'organizations'}"
        @click="selectedDictionary = 'organizations'"
        class="dictionary shadow_light"
      >
        <h2>Организации</h2>
      </div>
      <div
        :class="{ selected: selectedDictionary === 'mds_percents'}"
        @click="selectedDictionary = 'mds_percents'"
        class="dictionary shadow_light"
      >
        <h2>Процент МДС</h2>
      </div>
      <div
        :class="{ selected: selectedDictionary === 'extra_pay_variants'}"
        @click="selectedDictionary = 'extra_pay_variants'"
        class="dictionary shadow_light"
      >
        <h2>Варианты расчётов</h2>
      </div>
      <div
        :class="{ selected: selectedDictionary === 'min_extra_pay'}"
        @click="selectedDictionary = 'min_extra_pay'"
        class="dictionary shadow_light"
      >
        <h2>Минимальная доплата</h2>
      </div>
    </div>
    <div class="config-view shadow_medium">
      <div v-show="selectedDictionary === 'functions'" class="config_tab-content">
        <h1>Должности</h1>
        <Scrollable>
          <div class="row" v-for="func in functionsList" :key="func.id">
            <h4>{{func.name}}</h4>
            <EditButton class="row-button" @click="functionForEdit = func, showAddingPopup = true"></EditButton>
            <CommonButton
              :disabled="func.hasUsages"
              class="delete-button"
              text="удалить"
              @click="removeFunction(func.id)"
            ></CommonButton>
          </div>
        </Scrollable>
      </div>
      <div v-show="selectedDictionary === 'organizations'" class="config_tab-content">
        <h1>Организации</h1>
        <div class="row">
          <h5>Наименование</h5>
          <h5 class="center-align">Коэффициент</h5>
          <!-- <h5 style="flex: 0 0 158px;"></h5> -->
          <div class="space-wraper row-button"></div>
          <div class="space-wraper delete-button"></div>
        </div>
        <Scrollable>
          <div class="row" v-for="org in organizationsList" :key="org.id">
            <h4>{{org.organizationName}}</h4>
            <h4 class="center-align">{{org.multiplier}}</h4>
            <EditButton
              class="row-button"
              @click="organizationForEdit = org, showAddingPopup = true"
            ></EditButton>
            <CommonButton
              :disabled="org.hasUsages"
              class="delete-button"
              text="удалить"
              @click="removeOrganization(org.id)"
            ></CommonButton>
          </div>
        </Scrollable>
      </div>
      <div v-show="selectedDictionary === 'mds_percents'" class="config_tab-content">
        <h1>Процент МДС</h1>
        <div class="year-select">
          <h5>Год:</h5>
          <DropDown v-model="selectedYear" :list="mdsYears" />
          <PlusButton noBackground @click="addYear"></PlusButton>
        </div>
        <div class="row header-row">
          <h5>Стаж муниципальной службы</h5>
          <h5 class="center-align">Процент</h5>
          <h5 style="flex: 0 0 158px;"></h5>
        </div>
        <Scrollable>
          <div class="row" v-for="(perc, i) in selectedYearPercs" :key="i">
            <h4>{{perc.age}}</h4>
            <h4 class="center-align">{{perc.amount}}</h4>
            <CommonButton
              :disabled="!perc.allowEdit"
              class="delete-button"
              text="удалить"
              @click="removeMdsPercent(perc.id)"
            ></CommonButton>
          </div>
        </Scrollable>
      </div>
      <div
        v-show="selectedDictionary === 'extra_pay_variants'"
        class="config_tab-content extra-pay_tab"
      >
        <h1>Варианты расчётов</h1>
        <Scrollable>
          <LabeledBlock lined v-for="(variant, i) in extraPayVariants" :key="i">
            <template slot="title">
              <h2>Номер {{variant.number}}</h2>
            </template>
            <template slot="buttons">
              <CommonButton
                :disabled="!variant.editable"
                class="delete-button"
                text="удалить"
                @click="removeExtraPayVariants(variant.id)"
              ></CommonButton>
            </template>
            <template slot="content">
              <div class="extra-pay_row">
                <h5>Уральский к-т</h5>
                <h3>{{variant.uralMultiplier || '-'}}</h3>
              </div>
              <div class="extra-pay_row">
                <h5>Премия (%)</h5>
                <h3>{{variant.premiumPerc || '-'}}</h3>
              </div>
              <div class="extra-pay_row"></div>
              <div class="extra-pay_row">
                <h5>К-т мат. помощи</h5>
                <h3>{{variant.matSupportMultiplier || '-'}}</h3>
              </div>
              <div class="extra-pay_row">
                <h5>Выслуга (%)</h5>
                <h3>{{variant.vyslugaDivPerc || '-'}}</h3>
              </div>
              <div class="extra-pay_row">
                <h5>Учитывать госпенсию</h5>
                <h3>{{variant.ignoreGosPension ? 'Нет' : 'Да'}}</h3>
              </div>
            </template>
          </LabeledBlock>
        </Scrollable>
      </div>
      <div
        v-show="selectedDictionary === 'min_extra_pay'"
        class="config_tab-content min-extra-pay_tab"
      >
        <h1>Минимальная доплата</h1>
        <div class="min_extra_pay-row">
          <LabeledInput pattern="money" v-model="newMinExtraPay"></LabeledInput>
          <SaveButton @click="setMinExtraPay"></SaveButton>
        </div>
      </div>
    </div>
    <div class="command_menu">
      <PlusButton @click="showPopup" v-show="addingPopupTitle"></PlusButton>
    </div>
    <Popup
      name="adding-popup"
      :shown="showAddingPopup"
      :title="addingPopupTitle"
      @close="closePopup"
    >
      <div class="config_popup-slot">
        <template v-if="selectedDictionary === 'functions'">
          <template v-if="functionForEdit.id">
            <LabeledInput required v-model="functionForEdit.name" placeholder="Название"></LabeledInput>
            <div class="popup_br"></div>
            <CommonButton :disabled="!functionForEdit.name" text="Сохранить" @click="editFunction"></CommonButton>
          </template>
          <template v-else>
            <LabeledInput required v-model="newFunction" placeholder="Название"></LabeledInput>
            <div class="popup_br"></div>
            <CommonButton :disabled="!newFunction" text="Добавить" @click="createFunction"></CommonButton>
          </template>
        </template>
        <template v-if="selectedDictionary === 'organizations'">
          <template v-if="organizationForEdit.id">
            <div class="popup_input_block">
              <LabeledInput
                required
                v-model="organizationForEdit.organizationName"
                placeholder="Название"
              ></LabeledInput>
              <div class="popup_br"></div>
              <LabeledInput
                :deniedReg="/[^\d\.]/g"
                required
                v-model="organizationForEdit.multiplier"
                placeholder="Коэффициент"
              ></LabeledInput>
            </div>
            <CommonButton
              :disabled="!organizationForEdit.organizationName || !organizationForEdit.multiplier"
              text="Сохранить"
              @click="editOrganization"
            ></CommonButton>
          </template>
          <template v-else>
            <div class="popup_input_block">
              <LabeledInput required v-model="newOrganization" placeholder="Название"></LabeledInput>
              <div class="popup_br"></div>
              <LabeledInput
                :deniedReg="/[^\d\.]/g"
                required
                v-model="newOrganizationMultiplier"
                placeholder="Коэффициент"
              ></LabeledInput>
            </div>
            <CommonButton
              :disabled="!newOrganization || !newOrganizationMultiplier"
              text="Добавить"
              @click="createOrganization"
            ></CommonButton>
          </template>
        </template>
        <template v-if="selectedDictionary === 'mds_percents'">
          <LabeledInput
            :required="!newPercent.m"
            class="popup_number-input mds-percent"
            type="number"
            v-model="newPercent.y"
            placeholder="лет"
          ></LabeledInput>
          <LabeledInput
            :required="!newPercent.y"
            class="popup_number-input mds-percent"
            type="number"
            v-model="newPercent.m"
            placeholder="месяцев"
          ></LabeledInput>
          <LabeledInput
            required
            class="popup_number-input mds-percent"
            type="number"
            v-model="newPercent.perc"
            placeholder="процент"
          ></LabeledInput>
          <div class="popup_br"></div>
          <CommonButton
            :disabled="!newPercent.y && !newPercent.m || !newPercent.perc"
            text="Добавить"
            @click="createPerc"
          ></CommonButton>
        </template>
        <template v-if="selectedDictionary === 'extra_pay_variants'">
          <div>
            <LabeledInput
              required
              class="popup_number-input"
              type="number"
              v-model="newExPaVar.number"
              placeholder="Номер"
            ></LabeledInput>
            <LabeledInput
              class="popup_number-input"
              type="number"
              :deniedReg="/[^\d\.]/g"
              v-model="newExPaVar.uralMul"
              placeholder="Уральский к-т"
            ></LabeledInput>
            <LabeledInput
              class="popup_number-input"
              type="number"
              v-model="newExPaVar.premPerc"
              placeholder="Премия (%)"
            ></LabeledInput>
            <LabeledInput
              class="popup_number-input"
              type="number"
              v-model="newExPaVar.vysDivPerc"
              placeholder="Выслуга (%)"
            ></LabeledInput>
            <LabeledInput
              class="popup_number-input"
              type="number"
              :deniedReg="/[^\d\.]/g"
              v-model="newExPaVar.matSupMul"
              placeholder="Материальная помощь (к-т)"
            ></LabeledInput>
            <DropDown
              title="Учитывать госпенсию"
              v-model="newExPaVar.ignoreGosPension"
              :list="booleanReverseList"
            ></DropDown>
          </div>
          <CommonButton
            :disabled="!newExPaVar.number"
            text="Добавить"
            @click="createExtraPayVariant"
          ></CommonButton>
        </template>
      </div>
    </Popup>
    <Popup :shown="showYearPopup" title="Добавление года" @close="showYearPopup = false">
      <div class="config_popup-slot">
        <LabeledInput
          class="popup_number-input year"
          v-model="newYear"
          type="number"
          max-length="4"
          placeholder="Год"
        ></LabeledInput>
        <div class="popup_br"></div>
        <CommonButton :disabled="!newYear" text="Добавить" @click="createYear"></CommonButton>
      </div>
    </Popup>
  </div>
</template>

<style>
.config-view {
  display: flex;
  height: 100%;
  border-radius: 4px;
  background-color: #6254a3;
  box-sizing: border-box;
  flex-wrap: nowrap;
  flex: 0 0;
  flex-basis: calc(67% - 64px);
}

.dictionaries-list {
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
.dictionary {
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
.dictionary.selected {
  background-color: #503f96;
  border: 1px solid #3a3066;
}
.dictionary h2 {
  margin: 0;
}

.config_tab-content {
  padding: 0 48px;
  box-sizing: border-box;
  display: flex;
  flex: 0 0 100%;
  width: 100%;
  height: 100%;
  align-content: flex-start;
  align-items: baseline;
  flex-wrap: nowrap;
  flex-direction: column;
}
.config_tab-content .scrollable-container {
  width: calc(100% + 34px);
  height: 100%;
  margin-bottom: 48px;
  margin-right: -24px;
}
.config_tab-content.extra-pay_tab .scrollable-container {
  width: calc(100% + 18px);
  height: 100%;
  margin-bottom: 48px;
}
.config_tab-content h1 {
  margin: 48px 0 24px;
  flex: 0 0;
  width: 100%;
}
.config_tab-content h4,
.config_tab-content h5 {
  margin: 0;
  flex: 1 1 33%;
}
.config_tab-content .center-align {
  text-align: center;
}

.year-select {
  display: flex;
  width: 100%;
  align-content: center;
  align-items: center;
  flex-direction: row;
  margin-top: 16px;
  flex: 0 0 64px;
}
/* .year-select button,
.year-select input,
.year-select li,
.year-select .dropdown,
.year-select .dropdown-input,
 {
  margin: 0;
  height: 48px;
}
.year-select button,
.year-select li {
  padding: 15px 23px;
} */
.year-select h5 {
  flex: 0 0 80px;
}
.year-select .dropdown {
  flex: 0 0 160px;
}
.row {
  display: flex;
  flex-wrap: nowrap;
  justify-content: space-between;
  flex: 0 0 auto;
  width: 100%;
  border-bottom: 1px solid #534590;
  padding: 24px 0;
}
.row .space-wraper {
  box-sizing: border-box;
}
.row .delete-button {
  height: 24px;
  width: 112px;
  margin: -12px 0 -12px 24px;
  font-size: 12pt;
  flex: 0 0 auto;
}
.row .row-button {
  height: 48px;
  width: 48px;
  margin: -12px 0;
  padding: 16px;
  flex: 0 0 auto;
}
.header-row {
  margin-top: 16px;
}
.row:last-child {
  border-bottom: none;
}

.extra-pay_tab .labeled_block-content {
  padding: 12px 0 0;
}
.extra-pay_tab .extra-pay_row {
  margin: 0 0 24px;
  width: 33%;
  display: inline-flex;
  flex-wrap: wrap;
}
.extra-pay_tab .extra-pay_row h5 {
  margin: 0;
  flex: 0 0 100%;
  margin-right: 24px;
}
.extra-pay_tab .extra-pay_row h4 {
  margin: 0;
  flex: 0 0 auto;
  margin-right: 16px;
  font-size: 14pt;
  line-height: 20pt;
}
.extra-pay_tab .labeled_block-title h2 {
  margin: 16px 16px 16px 0;
  flex: 0 0 auto;
}

.min_extra_pay-row {
  display: flex;
  flex-direction: row;
  justify-content: flex-start;
}
.min_extra_pay-row * {
  margin-right: 24px;
}
.min_extra_pay-row .text-input {
  background-color: #503f96;
}

.config_popup-slot {
  text-align: center;
  margin: 0 auto;
  height: 100%;
  min-height: 274px;
  display: flex;
  flex-wrap: wrap;
  justify-content: center;
  justify-items: center;
  align-content: space-between;
}
.config_popup-slot .labeled-input {
  margin-top: 16px;
  width: 368px;
}
.config_popup-slot .labeled-input:first-child {
  margin-top: 0;
}
.config_popup-slot .popup_number-input {
  width: 33%;
  margin: 0 12px 12px;
}
.config_popup-slot .popup_number-input:last-child {
  margin-bottom: 0;
}
.config_popup-slot .popup_number-input.double {
  width: calc(66% + 24px);
}
.config_popup-slot .popup_number-input.mds-percent {
  width: 25%;
  margin: 0 12px;
}
.config_popup-slot .popup_number-input.year {
  width: 368px;
  margin: 0 12px;
}

.config_popup-slot .popup_input_block {
  width: 100%;
  flex: 0 0 100%;
}
.config_popup-slot .common-button {
  width: 240px;
  margin-top: 40px;
}
.config_popup-slot .dropdown {
  text-align: left;
  width: 33%;
  margin: 0 25% 12px;
}
.config_popup-slot .dropdown .common-button {
  width: 64px;
  margin-top: 0;
}

.labeled_block-title .delete-button {
  height: 48px;
  margin: 0 24px;
  font-size: 12pt;
}
</style>

<script lang="ts">
import Vue from 'vue';
import CommonButton from '@/components/CommonButton.vue';
import CrossButton from '@/components/Common/Buttons/CrossButton.vue';
import PlusButton from '@/components/Common/Buttons/PlusButton.vue';
import EditButton from '@/components/Common/Buttons/EditButton.vue';
import SaveButton from '@/components/Common/Buttons/SaveButton.vue';
import Scrollable from '@/components/Scrollable.vue';
import LabeledInput from '@/components/LabeledInput.vue';
import DropDown from '@/components/DropDown.vue';
import Popup from '@/components/Popup.vue';
import MdsPercents from '@/models/config/dictionaries/mdsPercent';
import LabeledBlock from '@/components/Common/LabeledBlock.vue';
import RequestSender from '@/requestSender';
import { floatParse } from '@/models/utils/calculations';
export default Vue.extend({
  data() {
    return {
      newFunction: '',
      functionForEdit: { id: '', name: '' },
      newOrganization: '',
      newOrganizationMultiplier: '',
      organizationForEdit: { id: '', organizationName: '', multiplier: '' },
      selectedDictionary: 'functions',
      selectedYear: '',
      newYear: '',
      newPercent: { y: '', m: '', d: '', perc: '' },
      newExPaVar: {
        id: '',
        number: '',
        uralMul: '',
        premPerc: '',
        matSupMul: '',
        vysDivPerc: '',
        ignoreGosPension: 'false',
      },
      newMinExtraPay: '',
      showAddingPopup: false,
      showYearPopup: false,
      booleanReverseList: [
        { value: 'false', title: 'Да' },
        { value: 'true', title: 'Нет' },
      ],
    };
  },
  computed: {
    requestSender(): RequestSender {
      return this.$store.getters.requestSender;
    },
    functionsList(): Array<{}> {
      return this.$store.getters.functionsList;
    },
    organizationsList(): Array<{}> {
      return this.$store.getters.organizationsList;
    },
    mdsPercentsList(): Array<{}> {
      return this.$store.getters.mdsPercentsList;
    },
    minExtraPay(): string {
      return this.$store.getters.minExtraPay;
    },
    extraPayVariants(): Array<{}> {
      return this.$store.getters.extraPayVariants;
    },
    addingPopupTitle(): string {
      switch (this.selectedDictionary) {
        case 'functions':
          return this.functionForEdit.id ? 'Редактировать должность' : 'Добавление должности';
        case 'organizations':
          return this.organizationForEdit.id ? 'Редактировать организацию' : 'Добавление организации';
        case 'mds_percents':
          return 'Добавление правила';
        case 'extra_pay_variants':
          return 'Добавление варианта расчётов';
        default:
          return '';
      }
    },
    selectedYearPercs(): Array<{}> {
      return (
        (
          (this.mdsPercentsList || []).find((el: any) =>
            el.year.toString() === this.selectedYear) || {} as any
        ).dsPercs || []
      ).map((elem: any) => new MdsPercents(elem)) as Array<{}>;
    },
    mdsYears(): Array<{}> {
      const mds = this.mdsPercentsList.map((el: any) => {
        return {
          value: el.year.toString(),
          title: el.year.toString(),
        };
      });
      if (!this.selectedYear && mds && mds[0]) {
        this.selectedYear = mds[0].value;
      }
      return mds;
    },
  },
  watch: {
    minExtraPay(val) {
      this.newMinExtraPay = val;
    },
  },
  mounted() {
    this.requestSender.getAllFunctions();
    this.requestSender.getAllOrganizations();
    this.requestSender.getAllMdsPercents();
    this.requestSender.getAllExtraPayVariants();
    this.requestSender.getMinExtraPay();
    this.newMinExtraPay = this.minExtraPay;
    this.selectedYear = new Date().getFullYear().toString();
  },
  methods: {
    addYear() {
      this.showYearPopup = true;
    },
    createYear() {
      const year = {
        year: parseInt(this.newYear, 10),
        dsPercs: [],
      };

      if (year.year) {
        this.$store.commit('setMdsPercentsList',
          this.mdsPercentsList.concat([year]));
        this.selectedYear = this.newYear;
      }
      this.showYearPopup = false;
    },
    createPerc() {
      const year = parseInt(this.selectedYear, 10);
      const newYear = {
        year,
        dsPercs: [] as any[],
      };
      const mdsYear = this.mdsPercentsList
        .find((el: any) => el.year === year) as any;
      if (mdsYear) {
        newYear.year = mdsYear.year;
        newYear.dsPercs = mdsYear.dsPercs.concat([]);
      }

      newYear.dsPercs.push({
        amount: parseInt(this.newPercent.perc, 10) || 0,
        ageYears: parseInt(this.newPercent.y, 10) || 0,
        ageMonths: parseInt(this.newPercent.m, 10) || 0,
        ageDays: parseInt(this.newPercent.d, 10) || 0,
      });
      this.requestSender.updateMdsPercent(newYear)
        .then(() => {
          this.showAddingPopup = false;
        });
    },
    createFunction() {
      this.requestSender.createFunction({
        name: this.newFunction,
      }).then(() => {
        this.showAddingPopup = false;
      });
    },
    editFunction() {
      this.requestSender.editFunction(this.functionForEdit)
        .then(() => {
          this.showAddingPopup = false;
        });
    },
    createOrganization() {
      this.requestSender.createOrganization({
        name: this.newOrganization,
        multiplier: this.newOrganizationMultiplier,
      }).then(() => {
        this.showAddingPopup = false;
      });
    },
    editOrganization() {
      this.requestSender.editOrganization({
        id: this.organizationForEdit.id,
        name: this.organizationForEdit.organizationName,
        multiplier: this.organizationForEdit.multiplier,
      }).then(() => {
        this.showAddingPopup = false;
      });
    },
    setMinExtraPay() {
      this.requestSender.setMinExtraPay(floatParse(this.newMinExtraPay));
    },
    removeFunction(id: string) {
      this.requestSender.removeFunction(id);
    },
    removeOrganization(id: string) {
      this.requestSender.removeOrganization(id);
    },
    removeMdsPercent(id: string) {
      this.requestSender.removeMdsPercent(id);
    },
    removeExtraPayVariants(id: string) {
      this.requestSender.removeExtraPayVariants(id);
    },
    createExtraPayVariant() {
      this.requestSender.createExtraPayVariant(this.newExPaVar)
        .then(() => {
          this.showAddingPopup = false;
        });
    },
    showPopup() {
      this.newFunction = '';
      this.functionForEdit = { id: '', name: '' };

      this.newOrganization = '';
      this.newOrganizationMultiplier = '';
      this.organizationForEdit = { id: '', organizationName: '', multiplier: '' };

      this.newPercent = { y: '', m: '', d: '', perc: '' };

      this.newExPaVar = {
        id: '',
        number: '',
        uralMul: '',
        premPerc: '',
        matSupMul: '',
        vysDivPerc: '',
        ignoreGosPension: 'false',
      };

      (window as any).vueChildren = this.$children;
      const popup = this.$children.find((child: any) => {
        return child.$attrs.name === 'adding-popup';
      });
      if (popup) {
        popup.$children.forEach((child: Vue) => {
          if (child.$data.hasOwnProperty('changed')) {
            child.$data.changed = false;
          }
        });
      }

      this.showAddingPopup = true;
    },
    closePopup() {
      this.showAddingPopup = false;
    },
  },
  components: {
    CommonButton,
    EditButton,
    CrossButton,
    PlusButton,
    SaveButton,
    Scrollable,
    LabeledInput,
    DropDown,
    Popup,
    LabeledBlock,
  },
});
</script>
