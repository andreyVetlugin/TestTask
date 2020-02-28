<template>
  <div class="page_layout egisso">
    <div class="dictionaries-list">
      <div
        :class="{ selected: selectedDictionary === 'periods'}"
        @click="selectedDictionary = 'periods'"
        class="dictionary shadow_light"
      >
        <h2>Периодичность МСЗ</h2>
      </div>
      <div
        :class="{ selected: selectedDictionary === 'measure'}"
        @click="selectedDictionary = 'measure'"
        class="dictionary shadow_light"
      >
        <h2>Единицы измерения</h2>
      </div>
      <div
        :class="{ selected: selectedDictionary === 'forms'}"
        @click="selectedDictionary = 'forms'"
        class="dictionary shadow_light"
      >
        <h2>Формы предоставления</h2>
      </div>
      <div
        :class="{ selected: selectedDictionary === 'recipient_codes'}"
        @click="selectedDictionary = 'recipient_codes'"
        class="dictionary shadow_light"
      >
        <h2>Коды получателя</h2>
      </div>
      <div
        :class="{ selected: selectedDictionary === 'soc_help'}"
        @click="selectedDictionary = 'soc_help'"
        class="dictionary shadow_light"
      >
        <h2>Меры социальной поддержки</h2>
      </div>
      <div
        :class="{ selected: selectedDictionary === 'print'}"
        @click="selectedDictionary = 'print'"
        class="dictionary shadow_light"
      >
        <h2>Выгрузка файлов</h2>
      </div>
    </div>
    <div class="config-view shadow_medium">
      <div v-show="!showEditTab" class="egisso_tab">
        <div v-show="selectedDictionary === 'periods'" class="egisso_tab-content">
          <h1>Периодичность МСЗ</h1>
          <Scrollable>
            <div class="egisso_row" v-for="period in egissoPeriods" :key="period.id">
              <h4>{{period.name}}</h4>
              <EditButton @click="openEditPeriod(period)"></EditButton>
            </div>
          </Scrollable>
        </div>
        <div v-show="selectedDictionary === 'measure'" class="egisso_tab-content">
          <h1>Единицы измерения</h1>
          <Scrollable>
            <div class="egisso_row" v-for="measure in measureUnits" :key="measure.id">
              <h4>{{measure.name}}</h4>
              <EditButton @click="openEditMeasureUnit(measure)"></EditButton>
            </div>
          </Scrollable>
        </div>
        <div v-show="selectedDictionary === 'forms'" class="egisso_tab-content">
          <h1>Формы предоставления</h1>
          <Scrollable>
            <div class="egisso_row" v-for="form in egissoForms" :key="form.id">
              <h4>{{form.name}}</h4>
              <EditButton @click="openEditEgissoForm(form)"></EditButton>
            </div>
          </Scrollable>
        </div>
        <div v-show="selectedDictionary === 'recipient_codes'" class="egisso_tab-content">
          <h1>Коды получателя</h1>
          <Scrollable>
            <div class="egisso_row" v-for="code in recipientCodes" :key="code.id">
              <h4>{{code.name}}</h4>
              <EditButton @click="openEditRecipientCode(code)"></EditButton>
            </div>
          </Scrollable>
        </div>
        <div v-show="selectedDictionary === 'soc_help'" class="egisso_tab-content">
          <h1>Меры социальной поддержки</h1>
          <Scrollable>
            <div class="egisso_row" v-for="socHelp in socialHelps" :key="socHelp.id">
              <h4>{{socHelp.name}}</h4>
              <EditButton @click="openEditSocialHelp(socHelp)"></EditButton>
            </div>
          </Scrollable>
        </div>
        <div v-show="selectedDictionary === 'print'" class="egisso_tab-content">
          <h1>Выгрузка файлов</h1>
          <Scrollable>
            <div class="egisso_row">
              <h4>Валидация</h4>
              <PrintButton @click="printEgissoValidation"></PrintButton>
            </div>
            <div class="egisso_row">
              <h4>Выгрузить факты</h4>
              <PrintButton @click="printPopup.show = true"></PrintButton>
            </div>
            <div class="print_history" v-if="printHistory && printHistory.length">
              <h3>История выгрузки фактов</h3>
              <div class="egisso_row">
                <h4>№</h4>
                <h4>Дата/время</h4>
                <h4>Начало действия</h4>
                <h4>Окончание действия</h4>
              </div>
              <div class="egisso_row">
                <h4>{{printHistory[0].number}}</h4>
                <h4>{{printHistory[0].data}}</h4>
                <h4>{{printHistory[0].start}}</h4>
                <h4>{{printHistory[0].end}}</h4>
              </div>
              <Expandable v-model="showPrintHistory">
                <template slot="title">
                  <h3>{{showPrintHistory ? 'Скрыть историю выгрузки' : 'Посмотреть историю выгрузки'}}</h3>
                </template>
                <template slot="content">
                  <div class="egisso_row" v-for="history in printHistory.slice(1)" :key="history.number">
                    <h4>{{history.number}}</h4>
                    <h4>{{history.data}}</h4>
                    <h4>{{history.start}}</h4>
                    <h4>{{history.end}}</h4>
                  </div>
                </template>
              </Expandable>
            </div>
          </Scrollable>
        </div>
      </div>
      <div v-show="showEditTab" class="egisso_tab-content egisso_edit">
        <h1>{{editTabTitle}}</h1>
        <template v-if="selectedDictionary === 'periods'">
          <Scrollable key="periods_scroll">
            <LabeledInput title="Наименование" v-model="newPeriod.name"></LabeledInput>
            <DropDown title="№ п/п" v-model="newPeriod.number" :list="tenList"></DropDown>
            <LabeledInput title="Код позиции" v-model="newPeriod.code" type="number" :maxlength="2"></LabeledInput>
          </Scrollable>
          <div class="button_block">
            <CommonButton text="Сохранить" @click="savePeriod"></CommonButton>
            <CommonButton text="Отмена" @click="showEditTab = false"></CommonButton>
          </div>
        </template>
        <template v-if="selectedDictionary === 'measure'">
          <Scrollable key="measure_scroll">
            <LabeledInput title="Наименование" v-model="newMeasureUnit.name" :maxlength="30"></LabeledInput>
            <LabeledInput
              title="Краткое наименование"
              v-model="newMeasureUnit.shortName"
              :maxlength="10"
            ></LabeledInput>
            <DropDown title="№ п/п" v-model="newMeasureUnit.number" :list="tenList"></DropDown>
            <LabeledInput
              title="Код позиции"
              v-model="newMeasureUnit.code"
              type="number"
              :maxlength="2"
            ></LabeledInput>
            <LabeledInput
              title="Код ОКЕИ"
              v-model="newMeasureUnit.okayCode"
              type="number"
              :maxlength="4"
            ></LabeledInput>
            <DropDown
              title="Представление"
              dropup
              v-model="newMeasureUnit.type"
              :list="measureTypeList"
            ></DropDown>
          </Scrollable>
          <div class="button_block">
            <CommonButton text="Сохранить" @click="saveMeasureUnit"></CommonButton>
            <CommonButton text="Отмена" @click="showEditTab = false"></CommonButton>
          </div>
        </template>
        <template v-if="selectedDictionary === 'forms'">
          <Scrollable key="forms_scroll">
            <LabeledInput title="Наименование" v-model="newEgissoForm.name" :maxlength="30"></LabeledInput>
            <LabeledInput
              title="Код формы "
              v-model="newEgissoForm.code"
              type="number"
              :maxlength="2"
            ></LabeledInput>
          </Scrollable>
          <div class="button_block">
            <CommonButton text="Сохранить" @click="saveEgissoForm"></CommonButton>
            <CommonButton text="Отмена" @click="showEditTab = false"></CommonButton>
          </div>
        </template>
        <template v-if="selectedDictionary === 'recipient_codes'">
          <Scrollable key="recipient_codes_scroll">
            <LabeledInput title="Наименование" v-model="newRecipientCode.name" :maxlength="1000"></LabeledInput>
            <LabeledInput
              title="Код получателя (КП)"
              v-model="newRecipientCode.code"
              pattern="99 99 99 99"
              key="recipient_code"
            ></LabeledInput>
          </Scrollable>
          <div class="button_block">
            <CommonButton text="Сохранить" @click="saveRecipientCode"></CommonButton>
            <CommonButton text="Отмена" @click="showEditTab = false"></CommonButton>
          </div>
        </template>
        <template v-if="selectedDictionary === 'soc_help'">
          <Scrollable key="soc_help_scroll">
            <LabeledInput title="Наименование" v-model="newSocialHelp.name" :maxlength="400"></LabeledInput>
            <LabeledInput
              title="Код в классификаторе ЕГИССО"
              v-model="newSocialHelp.code"
              type="number"
              :maxlength="4"
            ></LabeledInput>
            <DropDown
              title="Форма предоставления"
              v-model="newSocialHelp.formId"
              :list="egissoFormsList"
            ></DropDown>
            <DropDown
              title="Периодичность предоставления"
              v-model="newSocialHelp.periodId"
              :list="egissoPeriodsList"
            ></DropDown>
            <LabeledInput
              title="Идентификатор ЕГИССО"
              v-model="newSocialHelp.egissoId"
              :deniedReg="/[^\d\w\-]/g"
              :maxlength="40"
            ></LabeledInput>
            <!-- checkboxes  -->
            <div
              class="category_block"
              v-for="category in categoriesList"
              :key="category.categoryId"
            >
              <CheckBox
                :value="(newSocialHelp.categories || []).some((el) => el.categoryId === category.categoryId)"
                :title="category.name"
                @input="(val) => { if (val) {pushCategory(category.categoryId)} else {removeCategory(category.categoryId)}}"
              ></CheckBox>
              <DropDown
                v-show="(newSocialHelp.categories || []).some((el) => el.categoryId === category.categoryId)"
                title="Выплата"
                v-model="category.measureUnitId"
                :list="measureUnitsList"
              ></DropDown>
              <LabeledInput
                v-show="(newSocialHelp.categories || []).some((el) => el.categoryId === category.categoryId)"
                title="Идентификатор категории"
                v-model="category.egissoId"
                :deniedReg="/[^\d\w\-]/g"
                :maxlength="40"
              ></LabeledInput>
            </div>
            <!-- -->
            <DropDown
              title="Признак использования критериев нуждаемости при назначении МСЗ"
              v-model="newSocialHelp.isNeed"
              :list="booleanList"
              dropup
            ></DropDown>
            <DropDown
              title="Признак монетизации"
              v-model="newSocialHelp.isPayment"
              :list="booleanList"
              dropup
            ></DropDown>
          </Scrollable>
          <div class="button_block">
            <CommonButton text="Сохранить" @click="saveSocialHelp"></CommonButton>
            <CommonButton text="Отмена" @click="showEditTab = false"></CommonButton>
          </div>
        </template>
      </div>
    </div>
    <Popup title="Выгрузить факты" class="egisso_popup" v-show="printPopup.show" @close="printPopup.show = false">
      <h5 class="width-240 margin-left-50">Принятие решения о назначении</h5>
      <LabeledInput pattern="date" placeholder="дд.мм.гггг" v-model="printPopup.destinationDate"></LabeledInput>
      <div class="popup_br"></div>
      <h5 class="width-240 margin-left-50">Начало действия</h5>
      <LabeledInput pattern="date" placeholder="дд.мм.гггг" v-model="printPopup.from"></LabeledInput>
      <div class="popup_br"></div>
      <h5 class="width-240 margin-left-50">Окончание действия</h5>
      <LabeledInput pattern="date" placeholder="дд.мм.гггг" v-model="printPopup.to"></LabeledInput>
      <div class="popup_br"></div>
      <CommonButton text="Скачать" @click="printEgissoFacts" :disabled="cannotLoadEgissoFacts"></CommonButton>
      <CommonButton text="Отмена" @click="printPopup.show = false"></CommonButton>
    </Popup>
    <div class="command_menu">
      <CrossButton v-show="showEditTab" @click="showEditTab = false"></CrossButton>
      <PlusButton v-show="!showEditTab" @click="showEditTab = true"></PlusButton>
    </div>
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

.egisso_tab {
  width: 100%;
  height: 100%;
}
.egisso_tab-content {
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
.egisso_tab-content .scrollable-container {
  width: calc(100% + 34px);
  height: 100%;
  margin-bottom: 48px;
  margin-right: -24px;
}
.egisso_tab-content h1 {
  margin: 48px 0 24px;
  flex: 0 0;
  width: 100%;
}
.egisso_tab-content .center-align {
  text-align: center;
}
.egisso_row {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 12px 0;
  border-bottom: 1px solid #534590;
}
.egisso_row:last-child {
  border-bottom: none;
}
.egisso_row h4 {
  flex: 0 0 25%;
}

.print_history {
  margin-top: 32px;
}

/* edit tab */
.egisso_edit {
  background-color: #3f3176;
  border: 1px solid #493c80;
}
.egisso_tab-content.egisso_edit .scrollable-container {
  width: calc(100% + 34px);
  height: calc(100% - 112px);
  margin-bottom: 0;
  margin-right: -24px;
}
.egisso_tab-content.egisso_edit .scrollable-container.dropdown_scroll {
  max-height: 192px;
}
.egisso_edit .scrollable-content {
  display: flex;
  display: inline-flex;
  flex-direction: column;
}
.egisso_edit .dropdown,
.egisso_edit .labeled-input {
  width: 512px;
  margin: 12px 0;
}
.egisso_edit .labeled-input .text-input {
  background-color: #503f96;
}

.egisso_edit .category_block {
  margin-left: 16px;
  width: 600px;
}
.egisso_edit .button_block {
  width: 512px;
  margin: 40px 0 48px;
  display: flex;
  justify-content: space-between;
  align-content: center;
}

.egisso_edit .button_block .common-button {
  width: 240px;
}

/* popup */
.egisso_popup .popup-content .checkbox,
.egisso_popup .popup-content .labeled-input,
.egisso_popup .popup-content .common-button {
  margin-left: 16px;
  margin-right: 16px;
}
.egisso_popup .labeled-input {
  width: 240px;
  flex: 0 0 240px;
}
.egisso_popup .popup-content .common-button {
  width: 160px;
  flex: 0 0 160px;
}
.egisso_popup .popup-content .checkbox {
  width: auto;
  flex: 0 0 auto;
}
.egisso_popup .popup-content .labeled-input .text-input {
  background-color: #503f96;
}
</style>

<script lang="ts">
import Vue from 'vue';
import CommonButton from '@/components/CommonButton.vue';
import CrossButton from '@/components/Common/Buttons/CrossButton.vue';
import PlusButton from '@/components/Common/Buttons/PlusButton.vue';
import EditButton from '@/components/Common/Buttons/EditButton.vue';
import PrintButton from '@/components/Common/Buttons/PrintButton.vue';
import Scrollable from '@/components/Scrollable.vue';
import LabeledInput from '@/components/LabeledInput.vue';
import DropDown from '@/components/DropDown.vue';
import Expandable from '@/components/Expandable.vue';
import CheckBox from '@/components/Common/CheckBox.vue';
import Popup from '@/components/Popup.vue';
import RequestSender from '@/requestSender';
import EgissoPeriod from '@/models/egisso/egissoPeriod';
import MeasureUnit from '@/models/egisso/measureUnit';
import EgissoForm from '@/models/egisso/egissoForm';
import RecipientCode from '@/models/egisso/recipientCode';
import SocialHelp from '@/models/egisso/socialHelp';
import EgissoCategory from '@/models/egisso/egissoCategory';
import { formatDate } from '@/models/utils/stringBuilder';
import { AxiosResponse } from 'axios';
export default Vue.extend({
  data() {
    return {
      selectedDictionary: 'periods',
      showEditTab: false,
      showPrintHistory: false,
      printHistory: [],
      newPeriod: new EgissoPeriod({}),
      newMeasureUnit: new MeasureUnit({}),
      newEgissoForm: new EgissoForm({}),
      newRecipientCode: new RecipientCode({}),
      newSocialHelp: new SocialHelp({}),
      tenList: [
        { value: '1', title: '1' },
        { value: '2', title: '2' },
        { value: '3', title: '3' },
        { value: '4', title: '4' },
        { value: '5', title: '5' },
        { value: '6', title: '6' },
        { value: '7', title: '7' },
        { value: '8', title: '8' },
        { value: '9', title: '9' },
        { value: '10', title: '10' },
      ],
      measureTypeList: [
        { value: '0', title: 'Целое' },
        { value: '1', title: 'Десятичное' },
      ],
      booleanList: [
        { value: 'true', title: 'Да' },
        { value: 'false', title: 'Нет' },
      ],
      printPopup: {
        show: false,
        period: false,
        destinationDate: formatDate(new Date().toISOString()),
        from: formatDate(new Date().toISOString()),
        to: formatDate(new Date().toISOString()),
      },
    };
  },
  mounted() {
    this.requestSender.getEgissoPeriods();
  },
  computed: {
    requestSender(): RequestSender {
      return this.$store.getters.requestSender;
    },
    editTabTitle(): string { // TODO
      switch (this.selectedDictionary) {
        case 'periods':
          return this.newPeriod.id
            ? 'Редактировать периодичность предоставления МСЗ'
            : 'Новая периодичность предоставления МСЗ';
        case 'measure':
          return this.newMeasureUnit.id
            ? 'Редактировать единицу измерения'
            : 'Новая единица измерения';
        case 'forms':
          return this.newEgissoForm.id
            ? 'Редактировать форму предоставления'
            : 'Новая форма предоставления';
        case 'recipient_codes':
          return this.newRecipientCode.id
            ? 'Редактировать код получателя'
            : 'Новый код получателя';
        case 'soc_help':
          return this.newSocialHelp.id
            ? 'Редактировать меру социальной поддержки'
            : 'Новая мера социальной поддержки';
        default:
          return '';
      }
    },
    egissoPeriods(): EgissoPeriod[] {
      return this.$store.getters.egissoPeriods;
    },
    measureUnits(): MeasureUnit[] {
      return this.$store.getters.measureUnits;
    },
    egissoForms(): EgissoForm[] {
      return this.$store.getters.egissoForms;
    },
    recipientCodes(): RecipientCode[] {
      return this.$store.getters.recipientCodes;
    },
    socialHelps(): RecipientCode[] {
      return this.$store.getters.socialHelps;
    },
    egissoPeriodsList(): EgissoPeriod[] {
      return this.$store.getters.egissoPeriods.map((el: any) => {
        return {
          value: el.id,
          title: el.name,
        };
      });
    },
    measureUnitsList(): MeasureUnit[] {
      return this.$store.getters.measureUnits.map((el: any) => {
        return {
          value: el.id,
          title: el.name,
        };
      });
    },
    egissoFormsList(): EgissoForm[] {
      return this.$store.getters.egissoForms.map((el: any) => {
        return {
          value: el.id,
          title: el.name,
        };
      });
    },
    recipientCodesList(): RecipientCode[] {
      return this.$store.getters.recipientCodes.map((el: any) => {
        return {
          value: el.id,
          title: el.name,
        };
      });
    },
    categoriesList(): any[] {
      return this.recipientCodes.map((el: any) => {
        const result = this.newSocialHelp.categories.find((elem: any) => elem.categoryId === el.id);
        if (result) {
          result.name = (this.recipientCodes.find((elem) => elem.id === result.categoryId) || {} as any).name;
          return result;
        }
        return {
          categoryId: el.id,
          name: el.name,
          measureUnitId: '',
          egissoId: '',
        };
      });
    },
    cannotLoadEgissoFacts(): boolean {
      return !(this.printPopup.destinationDate && this.printPopup.from && this.printPopup.to);
    },
  },
  watch: {
    selectedDictionary(val) {
      switch (val) {
        case 'periods':
          this.requestSender.getEgissoPeriods();
          break;
        case 'measure':
          this.requestSender.getMeasureUnits();
          break;
        case 'forms':
          this.requestSender.getEgissoForms();
          break;
        case 'recipient_codes':
          this.requestSender.getRecipientCodes();
          break;
        case 'soc_help':
          this.requestSender.getEgissoPeriods();
          this.requestSender.getMeasureUnits();
          this.requestSender.getEgissoForms();
          this.requestSender.getRecipientCodes();
          this.requestSender.getSocialHelp();
          break;
        case 'print':
          this.requestSender.getPrintHistory()
          .then((resp: AxiosResponse) => {
            const count = resp.data.length;
            if (!count) { return; }
            this.printHistory = resp.data.map((el: any, i: number) => {
              return {
                  number: count - i,
                  data:  formatDate(el.data),
                  start: formatDate(el.start),
                  end:   formatDate(el.end),
                };
            });
          });
          break;
      }
      this.showEditTab = false;
    },
    showEditTab(val) {
      if (!val) {
        switch (this.selectedDictionary) {
          case 'periods':
            this.newPeriod.id = undefined;
            this.newPeriod.name = '';
            this.newPeriod.number = '1';
            this.newPeriod.code = '';
            break;
          case 'measure':
            this.newMeasureUnit.id = undefined;
            this.newMeasureUnit.name = '';
            this.newMeasureUnit.shortName = '';
            this.newMeasureUnit.number = '1';
            this.newMeasureUnit.code = '';
            this.newMeasureUnit.okayCode = '';
            this.newMeasureUnit.type = '';
            break;
          case 'forms':
            this.newEgissoForm.id = undefined;
            this.newEgissoForm.name = '';
            this.newEgissoForm.code = '';
            break;
          case 'recipient_codes':
            this.newRecipientCode.id = undefined;
            this.newRecipientCode.name = '';
            this.newRecipientCode.code = '';
            break;
          case 'soc_help':
            this.newSocialHelp.id = undefined;
            this.newSocialHelp.name = '';
            this.newSocialHelp.code = '';
            this.newSocialHelp.formId = '';
            this.newSocialHelp.periodId = '';
            this.newSocialHelp.egissoId = '';
            this.newSocialHelp.categories = [];
            this.newSocialHelp.isNeed = 'false';
            this.newSocialHelp.isPayment = 'false';
            break;
        }
      }
    },
  },
  methods: {
    savePeriod() {
      this.requestSender.createEditEgissoPeriod(this.newPeriod)
        .then(() => {
          this.showEditTab = false;
        });
    },
    openEditPeriod(period: EgissoPeriod) {
      this.newPeriod.fillFrom(period);
      this.showEditTab = true;
    },
    saveMeasureUnit() {
      this.requestSender.createEditMeasureUnit(this.newMeasureUnit)
        .then(() => {
          this.showEditTab = false;
        });
    },
    openEditMeasureUnit(unit: MeasureUnit) {
      this.newMeasureUnit.fillFrom(unit);
      this.showEditTab = true;
    },
    saveEgissoForm() {
      this.requestSender.createEditEgissoForm(this.newEgissoForm)
        .then(() => {
          this.showEditTab = false;
        });
    },
    openEditEgissoForm(form: EgissoForm) {
      this.newEgissoForm.fillFrom(form);
      this.showEditTab = true;
    },
    saveRecipientCode() {
      this.requestSender.createEditRecipientCode(this.newRecipientCode)
        .then(() => {
          this.showEditTab = false;
        });
    },
    openEditRecipientCode(code: RecipientCode) {
      this.newRecipientCode.fillFrom(code);
      this.showEditTab = true;
    },
    saveSocialHelp() {
      this.requestSender.createEditSocialHelp(this.newSocialHelp)
        .then(() => {
          this.showEditTab = false;
        });
    },
    openEditSocialHelp(socialHelp: SocialHelp) {
      this.newSocialHelp.fillFrom(socialHelp);
      this.showEditTab = true;
    },
    pushCategory(id: string) {
      this.newSocialHelp.categories.push(new EgissoCategory({ kpCodeId: id }));
    },
    removeCategory(id: string) {
      this.newSocialHelp.categories = this.newSocialHelp.categories.filter((el) => el.categoryId !== id);
    },
    printEgissoValidation() {
      this.requestSender.printEgissoValidation();
    },
    printEgissoFacts() {
      this.requestSender.printEgissoFacts(
        this.printPopup.destinationDate,
        this.printPopup.from,
        this.printPopup.to,
      ).then(() => {
        this.printPopup.show = false;
      });
    },
  },
  components: {
    CommonButton,
    CrossButton,
    EditButton,
    PlusButton,
    PrintButton,
    Scrollable,
    LabeledInput,
    DropDown,
    CheckBox,
    Popup,
    Expandable,
  },
});
</script>
