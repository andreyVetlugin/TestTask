<template>
  <div class="page_layout registry_layout">
    <div class="registry">
      <div class="registry_form shadow_medium">
        <div class="form-block">
          <DropDown disabled tiled v-model="selectedYear" :list="years"></DropDown>
          <DropDown disabled tiled v-model="selectedMonth" :list="months"></DropDown>
          <LabeledInput
            pattern="99.99.9999"
            placeholder="дд.мм.гггг"
            v-model="registry.registry.date"
          ></LabeledInput>
        </div>
        <div class="form-block right">
          <CommonButton
            title="Сформировать"
            :text="isLoaded ? 'Сформировать заново' : 'Сформировать'"
            @click="getForceNewRegistry"
          ></CommonButton>
        </div>
      </div>
      <div v-show="isLoaded" class="registry_table">
        <div class="registry_row">
          <h5 class="registry_row-number">№ п/п</h5>
          <!-- <h5 class="registry_row-number">№ п/р</h5> -->
          <h5 class="registry_row-fio">Ф И О</h5>
          <h5 class="registry_row-bank_card">Номер счета</h5>
          <h5 class="registry_row-sum">Сумма</h5>
          <CommonButton
            title="пересчитать всё"
            :text="'пересчитать всё'"
            @click="confirmRecountRow('')"
          ></CommonButton>
        </div>
        <Scrollable>
          <div class="registry_row" v-for="(row, i) in registry.rows" :key="i">
            <h4 class="registry_row-number">{{i+1}}</h4>
            <!-- <h4 class="registry_row-number">{{row.number}}</h4> -->
            <h4 class="registry_row-fio">{{row.fio}}</h4>
            <h4 class="registry_row-bank_card">{{row.account}}</h4>
            <h3 class="registry_row-sum">
              {{row.summ}}
              <span class="currency">&#8381;</span>
            </h3>
            <div class="button-block">
              <CommonButton
                noBackground
                class="delete-button"
                html="<i class='fa fa-times'></i>"
                @click="confirmDeleteRow(row.id)"
              ></CommonButton>
              <EditButton title="пересчитать" @click="confirmRecountRow(row.id)"></EditButton>
            </div>
          </div>
        </Scrollable>
      </div>
      <div v-if="isLoaded" class="registry_form shadow_medium">
        <div class="form-block wrapped_block">
          <h5>Всего фактов</h5>
          <h4>{{registry.count}}</h4>
          <h5>Итоговая сумма</h5>
          <h3>
            {{registry.summ}}
            <span class="currency">&#8381;</span>
          </h3>
        </div>
        <div class="form-block right">
          <CommonButton title="Выгрузить" :text="'Выгрузить'" @click="complete"></CommonButton>
          <CommonButton title="Выгрузить" :text="'Выгрузить на подпись'" @click="complete(true)"></CommonButton>
        </div>
      </div>
    </div>
    <Popup :shown="showDeletePopup" title="Подтверждение" @close="closeDeletePopup">
      <template>
        <h2>Вы действительно хотете удалить строку?</h2>
        <div class="popup_br"></div>
        <CommonButton :text="'Удалить'" @click="deleteElement"></CommonButton>
        <CommonButton :text="'Отмена'" @click="closeDeletePopup"></CommonButton>
      </template>
    </Popup>
    <Popup :shown="showRecountPopup" :title="recountPopupTitle" @close="closeRecountPopup">
      <LabeledInput
        pattern="99.99.9999"
        placeholder="дд.мм.гггг"
        v-model="recountData.from"
        @blur="changeDateInput"
      ></LabeledInput>
      <h4 class="date_delimeter">—</h4>
      <LabeledInput
        pattern="99.99.9999"
        placeholder="дд.мм.гггг"
        v-model="recountData.to"
        @blur="changeDateInput"
      ></LabeledInput>
      <div class="popup_br"></div>
      <LabeledInput
        pattern="money"
        class="summ_input"
        placeholder="сумма"
        v-show="rowForRecount"
        v-model="recountData.summ"
      ></LabeledInput>
      <span v-show="rowForRecount" class="currency">&#8381;</span>
      <div class="popup_br"></div>
      <CommonButton :text="'Принять'" @click="recount"></CommonButton>
      <CommonButton :text="'Отмена'" @click="closeRecountPopup"></CommonButton>
    </Popup>

    <RequestResultPopup @close="requestResult.showPopup = false" :requestResult="requestResult"></RequestResultPopup>

    <div class="command_menu"></div>
  </div>
</template>

<style>
.registry {
  display: flex;
  flex-wrap: nowrap;
  flex: 0 0;
  flex-basis: calc(100% - 112px);
  flex-direction: column;
  justify-content: space-between;
  align-items: center;
  margin: 0;
  margin-left: 16px;
}
.registry_form {
  background-color: #493c80;
  border-radius: 4px;
  flex: 0 0 auto;
  width: 100%;
  box-sizing: border-box;
  padding: 24px;
  display: flex;
  justify-content: space-between;
  align-items: flex-end;
}
.registry_form .form-block {
  flex: 1 1 auto;
  height: 64px;
  display: flex;
  flex-wrap: nowrap;
}
.registry_form .form-block.wrapped_block {
  flex-wrap: wrap;
}
.form-block.right {
  justify-content: flex-end;
}
.form-block > * {
  flex-basis: 180px;
  margin: 0 12px;
}
.form-block > .common-button {
  flex-basis: auto !important;
  width: max-content;
}
.form-block > *:first-child {
  margin-left: 0;
}
.form-block.right > *:last-child {
  margin-right: 0;
}
.form-block h3,
.form-block h4 {
  flex: 1 1;
  flex-basis: calc(100% - 160px);
  margin: 0;
}
.form-block h5 {
  flex: 0 0 160px;
  margin: 0;
}

.registry_table {
  width: 100%;
  height: calc(100% - 224px);
  flex: 1 1;
  flex-basis: calc(100% - 272px);
  flex-wrap: nowrap;
  display: flex;
  flex-direction: column;
}
.registry_table .scrollable-container {
  height: 100%;
  width: calc(100% + 36px);
}

.registry_row {
  width: 100%;
  flex: 0 0 64px;
  border-bottom: 1px;
  display: flex;
  flex-direction: row;
  justify-content: space-between;
  align-items: center;
  margin: 12px 0;
}
.registry_row > * {
  margin: 0 12px;
  word-break: break-word;
}
.registry_row .registry_row-number {
  flex: 0 0 80px;
}
.registry_row .registry_row-fio {
  flex: 0 0;
  flex-basis: calc(100% - 844px);
}
.registry_row .registry_row-bank_card {
  flex: 0 0 240px;
}
.registry_row .registry_row-sum {
  flex: 0 0 120px;
}
.registry_row .common-button:not(.no-text) {
  flex: 0 0 180px;
  font-size: 12pt;
}
.registry_row .button-block {
  flex: 0 0 180px;
  display: flex;
  justify-content: space-around;
}

.popup_slot {
  width: 100%;
  display: flex;
  flex-wrap: wrap;
  justify-content: center;
  align-items: center;
}
.popup_slot h2 {
  margin: 0 0 24px;
}
.popup_slot h4 {
  margin: 0 24px 24px;
  display: inline-block;
}
.popup_slot .common-button {
  margin: 0 12px;
  width: 160px;
}
.popup_slot .text-input {
  margin: 0 0 24px;
}

.registry_layout .popup-content .labeled-input {
  flex: 0 0 200px;
}
.registry_layout .popup-content .common-button:not(.no-text) {
  margin-right: 32px;
  flex: 0 0 160px;
}
.registry_layout .popup-content .common-button:not(.no-text):last-of-type {
  margin-right: 0;
}
.registry_layout .popup-content .summ_input {
  margin-left: 25%;
}
.registry_layout .popup-content .currency {
  margin: 16px 25% 0 16px;
  font-size: 16pt;
  color: #6253a2;
}

@media (max-width: 1440px) {
  .registry_row .registry_row-number {
    flex: 0 0 50px;
  }
  .registry_row .registry_row-fio {
    flex: 0 0;
    flex-basis: calc(100% - 724px);
  }
  .registry_row .registry_row-bank_card {
    flex: 0 0 160px;
  }
}
@media (max-width: 1024px) {
  .registry_layout {
    margin-top: 0;
    margin-bottom: 0;
    height: 100%;
  }
  .registry_form .form-block {
    height: 48px;
  }
  .registry_row {
    flex-basis: 48px;
  }
  .registry_row .registry_row-fio {
    flex-basis: calc(100% - 658px);
  }
  .registry_row .registry_row-bank_card {
    flex: 0 0 200px;
  }
  .registry_layout .registry input,
  .registry_layout .registry .dropdown-input,
  .registry_layout .registry .dropdown-container,
  .registry_layout .registry .common-button {
    height: 48px;
    font-size: 12pt;
  }
  .registry_layout .registry input,
  .registry_layout .registry .common-button {
    padding: 16px;
  }
  .registry_layout .registry .common-button.no-text {
    width: 48px;
  }
  .registry_layout .registry .dropdown .text-input {
    width: calc(100% - 48px);
    overflow: visible;
  }
}
</style>

<script lang="ts">
import Vue from 'vue';
import moment from 'moment';
import LabeledInput from '@/components/LabeledInput.vue';
import DropDown from '@/components/DropDown.vue';
import CommonButton from '@/components/CommonButton.vue';
import EditButton from '@/components/Common/Buttons/EditButton.vue';
import RequestResultPopup from '@/components/Common/Popups/RequestResultPopup.vue';
import Scrollable from '@/components/Scrollable.vue';
import Popup from '@/components/Popup.vue';
import Registry from '@/models/registry/registry';
import RequestSender from '@/requestSender';
import RegistryRow from '@/models/registry/registryRow';
import { formatMoney, formatDate } from '@/models/utils/stringBuilder';
import { AxiosError } from 'axios';
export default Vue.extend({
  data() {
    return {
      selectedYear: '',
      years: [
        { value: '2018', title: '2018' },
      ],
      selectedMonth: '',
      months: [
        { value: '1', title: 'январь' },
        { value: '2', title: 'февраль' },
        { value: '3', title: 'март' },
        { value: '4', title: 'апрель' },
        { value: '5', title: 'май' },
        { value: '6', title: 'июнь' },
        { value: '7', title: 'июль' },
        { value: '8', title: 'август' },
        { value: '9', title: 'сентябрь' },
        { value: '10', title: 'октябрь' },
        { value: '11', title: 'ноябрь' },
        { value: '12', title: 'декабрь' },
      ],
      dayInMonths: [0, 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31],
      registry: new Registry({}),
      recountData: {
        from: '',
        to: '',
        summ: '',
        baseSumm: 0,
      },
      showRecountPopup: false,
      rowForRecount: ' ',
      showDeletePopup: false,
      rowForDelete: ' ',
      requestResult: {
        success: true,
        message: '',
        showPopup: false,
      },
    };
  },
  mounted() {
    const today = moment().locale('ru');
    this.selectedYear = today.year().toString();
    this.selectedMonth = (today.month() + 1).toString();
    const max = today.year() + 10;
    for (let i = 2019; i < max || this.years.length % 3 !== 0; i++) {
      const next = i.toString();
      this.years.push({
        value: next,
        title: next,
      });
    }
    this.getRegistry();
  },
  computed: {
    requestSender(): RequestSender {
      return this.$store.getters.requestSender;
    },
    registryDate(): string {
      return this.registry.registry.date;
    },
    isLoaded(): boolean {
      return this.registry && this.registry.registry.id ? true : false;
    },
    recountPopupTitle(): string {
      return this.rowForRecount ? 'Перерасчёт для выбранной строки' : 'Перерасчет для всех строк';
    },
  },
  watch: {
    registryDate() {
      this.registry.registry.initDate = this.registry.registry.date;
      const parts = this.registry.registry.date.split('.');
      this.selectedYear = parts[2];
      this.selectedMonth = parseInt(parts[1], 10).toString();
    },
    rowForRecount() {
      const founded = this.registry.rows.find((row) => row.id === this.rowForRecount);
      if (founded) {
        this.recountData.from = founded.from;
        this.recountData.to = founded.to;
        this.recountData.summ = founded.summ;
        this.recountData.baseSumm = founded.baseSumm;
      } else {
        this.recountData.from = formatDate(`${this.selectedYear}-${this.selectedMonth}-01`);
        this.recountData.to =
          formatDate(`${this.selectedYear}-${this.selectedMonth}-${moment(this.registryDate).daysInMonth()}`);
        this.recountData.summ = '';
      }
    },
  },
  methods: {
    getRegistry() {
      this.requestSender.getRegistry().then((resp) => {
        if (resp.data) {
          this.registry.fillFrom(new Registry(resp.data));
        }
      });
    },
    getForceNewRegistry() {
      this.requestSender.getForceNewRegistry(this.registry.registry).then((resp) => {
        this.registry.fillFrom(new Registry(resp.data));
      });
    },
    complete(forSignature = false) {
      this.requestSender.completeRegistry(
        this.registry.registry.id,
        formatDate(this.registry.registry.date),
        forSignature)
        .catch((resp: AxiosError) => {
          this.requestResult.success = false;
          this.requestResult.message = resp.response ? resp.response.data.message : 'Ошибка выгрузки данных';
          this.requestResult.showPopup = true;
        });
    },
    confirmDeleteRow(id: string) {
      this.rowForDelete = id;
      this.showDeletePopup = true;
    },
    closeDeletePopup() {
      this.rowForDelete = '';
      this.showDeletePopup = false;
    },
    deleteElement() {
      this.requestSender.deleteRegistryElement(this.rowForDelete)
        .then(() => {
          this.requestSender.getRegistryReestrElements(this.registry.registry.id)
            .then((resp) => {
              this.registry.fillFrom(new Registry(resp.data));
            });
          this.closeDeletePopup();
        });
    },
    confirmRecountRow(id: string) {
      this.rowForRecount = id;
      this.showRecountPopup = true;
    },
    closeRecountPopup() {
      this.rowForRecount = ' ';
      this.showRecountPopup = false;
    },
    recount() {
      if (this.rowForRecount) {
        this.requestSender.reCountRegistryElement({
          reestrElementId: this.rowForRecount,
          from: this.recountData.from,
          to: this.recountData.to,
          newSumm: this.recountData.summ.replace(/ /g, ''),
        }).then(() => {
          this.requestSender.getRegistryReestrElements(this.registry.registry.id)
            .then((resp) => {
              this.registry.fillFrom(new Registry(resp.data));
            });
          this.closeRecountPopup();
        });
      } else {
        this.recountAll(this.recountData.from, this.recountData.to);
      }
    },
    recountAll(from: string, to: string) {
      this.requestSender.reCountAllRegistryElements(this.registry.rows.map((row) => {
        return {
          reestrElementId: row.id,
          from: this.recountData.from,
          to: this.recountData.to,
          newSumm: this.summRecount(from, to, row.baseSumm),
        };
      })).then(() => {
        this.requestSender.getRegistryReestrElements(this.registry.registry.id)
          .then((resp) => {
            this.registry.fillFrom(new Registry(resp.data));
          });
        this.closeRecountPopup();
      });
    },
    summRecount(from: string, to: string, baseSumm: number): number {
      return (
        (Math.abs(moment(from, 'DD.MM.YYYY').diff(moment(to, 'DD.MM.YYYY'), 'days')) + 1) /
        moment(this.registryDate, 'DD.MM.YYYY').daysInMonth() *
        baseSumm
      );
    },
    changeDateInput() {
      if (this.rowForRecount && this.recountData.from && this.recountData.to && this.recountData.baseSumm) {
        this.recountData.summ =
          formatMoney(this.summRecount(this.recountData.from, this.recountData.to, this.recountData.baseSumm));
      }
    },
  },
  components: {
    LabeledInput,
    DropDown,
    CommonButton,
    EditButton,
    Scrollable,
    Popup,
    RequestResultPopup,
  },
});
</script>
