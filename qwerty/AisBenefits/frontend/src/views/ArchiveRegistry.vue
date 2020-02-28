<template>
  <div class="page_layout registry_layout">
    <div class="registry_archive">
      <div class="registry_form shadow_medium">
        <div class="form-block">
          <LabeledInput type="number" pattern="9999" v-model="selectedYear"></LabeledInput>
          <MonthDropDown v-model="selectedMonth"></MonthDropDown>
        </div>
        <div class="form-block right">
          <CommonButton title="Открыть" :text="'Открыть'" @click="getRegistry"></CommonButton>
        </div>
      </div>
      <div v-show="isLoaded" class="registry_table">
        <div class="registry_row">
          <h5 class="registry_row-number">№ п/п</h5>
          <!-- <h5 class="registry_row-number">№ п/р</h5> -->
          <h5 class="registry_row-fio">Ф И О</h5>
          <h5 class="registry_row-bank_card">Номер счета</h5>
          <h5 class="registry_row-sum">Сумма</h5>
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

    <div class="command_menu"></div>

    <Popup
      class="registry_select_popup"
      title="Выберите реестр"
      v-show="showPopup"
      @close="showPopup=false"
    >
      <template>
        <div class="popup_row" v-for="reg in registries" :key="reg.id">
          <h4>№ {{reg.number}}</h4>
          <h4>От {{reg.date}}</h4>
          <CommonButton @click="loadRegistry(reg.id)" text="Выбрать"></CommonButton>
        </div>
      </template>
    </Popup>

    <RequestResultPopup @close="requestResult.showPopup = false" :requestResult="requestResult"></RequestResultPopup>
  </div>
</template>

<style>
.registry_archive {
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
  flex-basis: auto;
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
  /* text-align: center; */
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
  /* text-align: right; */
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

.registry_select_popup .popup-content {
  align-content: flex-start;
}

.popup_row {
  width: 100%;
  text-align: center;
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 24px;
}
.popup_row:last-child {
  margin-bottom: 12px;
}
.popup_row h4 {
  margin: 0 24px;
  width: auto;
  display: inline-block;
}
.popup_row .common-button {
  margin: 0 12px;
  width: 160px;
}
.popup-content .common-button {
  width: 160px;
}
</style>

<script lang="ts">
import Vue from 'vue';
import moment from 'moment';
import LabeledInput from '@/components/LabeledInput.vue';
import Popup from '@/components/Popup.vue';
import DropDown from '@/components/DropDown.vue';
import MonthDropDown from '@/components/Common/MonthDropDown.vue';
import CommonButton from '@/components/CommonButton.vue';
import Scrollable from '@/components/Scrollable.vue';
import Registry from '@/models/registry/registry';
import RequestSender from '@/requestSender';
import RegistryRow from '@/models/registry/registryRow';
import RequestResultPopup from '@/components/Common/Popups/RequestResultPopup.vue';
import { formatMoney, formatDate } from '@/models/utils/stringBuilder';
import { AxiosError } from 'axios';
export default Vue.extend({
  data() {
    return {
      selectedYear: '',
      selectedMonth: '',
      registry: new Registry({}),
      registries: [],
      showPopup: false,
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
    this.selectedMonth = today.format('MM');
  },
  computed: {
    requestSender(): RequestSender {
      return this.$store.getters.requestSender;
    },
    isLoaded(): boolean {
      return this.registry && this.registry.registry.id ? true : false;
    },
  },
  methods: {
    getRegistry() {
      this.requestSender.getRegistryArchive(parseInt(this.selectedYear, 10), parseInt(this.selectedMonth, 10))
        .then((resp) => {
          if (!resp.data || !resp.data.length) {
            this.requestResult.success = true;
            this.requestResult.message = 'Ничего не найдено';
            this.requestResult.showPopup = true;
            return;
          }
          if (resp.data.length === 1) {
            this.loadRegistry(resp.data[0].id);
            return;
          }

          this.registries = resp.data.map((el: any) => {
            return {
              number: el.number,
              date: formatDate(el.date),
              id: el.id,
            };
          });
          this.showPopup = true;
        });
    },
    loadRegistry(id: string) {
      this.requestSender.getRegistryReestrElements(id)
        .then((resp) => {
          this.registry.fillFrom(new Registry(resp.data));
          this.showPopup = false;
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
  },
  components: {
    LabeledInput,
    DropDown,
    CommonButton,
    Scrollable,
    MonthDropDown,
    Popup,
    RequestResultPopup,
  },
});
</script>
