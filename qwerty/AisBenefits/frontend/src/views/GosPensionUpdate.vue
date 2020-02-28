<template>
  <div class="page_layout pension_update_layout">
    <div class="pension_update">
      <div class="pension_update_form shadow_medium">
        <div class="form_block">
          <CommonButton title="Выбрать все" :text="'Выбрать все'" @click="selectAllRows()"></CommonButton>
          <CommonButton title="Снять все" :text="'Снять все'" @click="selectNoRows()"></CommonButton>
        </div>
        <div class="form_block right">
          <CommonButton
            title="Получить данные из ПФР"
            :text="'Получить данные из ПФР'"
            @click="syncAllGosPension()"
          ></CommonButton>
        </div>
      </div>
      <div class="pension_update_table" style="height: 100%">
        <div class="pension_update_row header_row">
          <i style="width: 34px;"></i>
          <h5 class="pension_update_row-number">Номер п/п</h5>
          <h5 class="pension_update_row-fio">ФИО</h5>
          <h5 class="pension_update_row-summ">Текущая пенсия</h5>
          <h5 class="pension_update_row-summ">Новая пенсия</h5>
          <h5 class="pension_update_row-summ">Разница в процентах</h5>
          <div class="button_block">
            <h5 class="pension_update_row-number">действия</h5>
          </div>
        </div>
        <Scrollable v-if="rows">
          <div class="pension_update_row" v-for="(row, i) in rows" :key="i">
            <div @click="row.selected = !row.selected" :class="{ selected: row.selected }">
              <i class="fa fa-check"></i>
            </div>
            <h4 class="pension_update_row-number">{{i+1}}</h4>
            <h4 class="pension_update_row-fio">{{row.fio}}</h4>
            <h3 class="pension_update_row-summ">
              {{row.currentPension}}
              <span class="currency">&#8381;</span>
            </h3>
            <h3 class="pension_update_row-summ">
              {{row.newPension}}
              <span class="currency">&#8381;</span>
            </h3>
            <h4 class="pension_update_row-summ">
              {{row.diff}}
              <span class="currency">%</span>
            </h4>
            <div class="button_block">
              <CommonButton
                noBackground
                class="delete-button"
                html="<i class='fa fa-times'></i>"
                @click="confirmDeclineOne(row.id)"
                title="Отклонить"
              ></CommonButton>
              <SaveButton noBackground title="Подтвердить" @click="confirmApproveOne(row.id)"></SaveButton>
            </div>
          </div>
        </Scrollable>
      </div>
      <div class="pension_update_form shadow_medium" v-if="rows">
        <div class="form_block">
          <CommonButton
            @click="confirmDeclineSelected()"
            title="Отклонить выбранное"
            text="Отклонить выбранное"
          ></CommonButton>
          <CommonButton
            @click="confirmApproveSelected()"
            title="Подтвердить выбранное"
            text="Подтвердить выбранное"
          ></CommonButton>
        </div>
        <div class="form_block right"></div>
      </div>
    </div>
    <Popup
      class="decline_popup"
      :shown="showDeclinePopup"
      title="Подтвердите отклонение"
      @close="closeDeclinePopup"
    >
      <div class="popup_br"></div>
      <CommonButton title="Отклонить" text="Отклонить" @click="declineRows"></CommonButton>
      <CommonButton title="Отмена" text="Отмена" @click="closeDeclinePopup"></CommonButton>
    </Popup>
    <Popup :shown="showApprovePopup" title="Подтвердить" @close="closeApprovePopup">
      <h5 class="width-140 margin-left-50">Назначение</h5>
      <LabeledInput
        class="width-240"
        pattern="99.99.9999"
        placeholder="дд.мм.гггг"
        v-model="newSolution.destination"
      ></LabeledInput>
      <div class="popup_br"></div>
      <h5 class="width-140 margin-left-50">Исполнение</h5>
      <LabeledInput
        class="width-240"
        pattern="99.99.9999"
        placeholder="дд.мм.гггг"
        v-model="newSolution.execution"
      ></LabeledInput>
      <div class="popup_br"></div>
      <h5 class="width-140 margin-left-50">Комментарий</h5>
      <LabeledInput class="width-240" v-model="newSolution.comment" placeholder="комментарий"></LabeledInput>
      <div class="popup_br"></div>
      <CommonButton
        @click="approveRows"
        :disabled="!newSolution.destination || !newSolution.execution"
        :text="'Подтвердить'"
      ></CommonButton>
    </Popup>

    <RequestResultPopup @close="requestResult.showPopup = false" :requestResult="requestResult"></RequestResultPopup>

    <div class="command_menu"></div>
  </div>
</template>

<style>
.pension_update {
  display: flex;
  flex-wrap: nowrap;
  flex: 0 0;
  flex-basis: calc(100% - 112px);
  flex-direction: column;
  justify-content: space-between;
  align-items: center;
  margin: 0;
  margin-left: 16px;
  height: 100%;
}
.pension_update_form {
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
.pension_update_form .form_block {
  flex: 1 1 auto;
  height: 64px;
  display: flex;
  flex-wrap: nowrap;
}
.pension_update_form .form_block.wrapped_block {
  flex-wrap: wrap;
}
.form_block.right {
  justify-content: flex-end;
}
.form_block > * {
  flex-basis: 180px;
  margin: 0 12px;
}
.form_block > .common-button {
  flex-basis: auto;
}
.form_block > *:first-child {
  margin-left: 0;
}
.form_block.right > *:last-child {
  margin-right: 0;
}
.form_block h3,
.form_block h4 {
  flex: 1 1;
  flex-basis: calc(100% - 160px);
  margin: 0;
}
.form_block h5 {
  flex: 0 0 160px;
  margin: 0;
}

.pension_update_table {
  width: 100%;
  flex: 1 1;
  flex-basis: calc(100% - 272px);
  flex-wrap: nowrap;
  display: flex;
  flex-direction: column;
}
.pension_update_table .scrollable-container {
  height: 100%;
  width: calc(100% + 36px);
}

.pension_update_row {
  width: 100%;
  flex: 0 0 64px;
  border-bottom: 1px;
  display: flex;
  flex-direction: row;
  justify-content: space-between;
  align-items: center;
  margin: 12px 0;
}
.pension_update_row > * {
  margin: 0 12px;
  word-break: break-word;
}
.pension_update_row.header_row h5 {
  text-align: center;
}
.pension_update_row .pension_update_row-number {
  flex: 0 0 80px;
  text-align: center;
}
.pension_update_row .pension_update_row-fio {
  flex: 0 0;
  flex-basis: calc(100% - 828px);
}
.pension_update_row.header_row .pension_update_row-fio {
  text-align: left;
}
.pension_update_row .pension_update_row-summ {
  flex: 0 0 120px;
  text-align: right;
}
.pension_update_row .button_block {
  flex: 0 0 180px;
  display: flex;
  justify-content: space-around;
}
.pension_update_row .fa-check {
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
.pension_update_row .fa-check:hover {
  background-color: #8a7ec0;
}
.pension_update_row .fa-check:active {
  background-color: #6253a2;
  color: #ffffff;
}
.pension_update_row .selected .fa-check {
  color: #ffffff;
}

.decline_popup .common-button:not(.no-text) {
  margin-right: 24px;
}
.decline_popup .common-button:not(.no-text):last-of-type {
  margin-right: 0;
}
</style>

<script lang="ts">
import Vue from 'vue';
import CommonButton from '@/components/CommonButton.vue';
import Scrollable from '@/components/Scrollable.vue';
import SaveButton from '@/components/Common/Buttons/SaveButton.vue';
import LabeledInput from '@/components/LabeledInput.vue';
import Popup from '@/components/Popup.vue';
import RequestSender from '@/requestSender';
import PensionRow from '@/models/pensionUpdate/pensionRow';
import RequestResultPopup from '@/components/Common/Popups/RequestResultPopup.vue';
import { formatDate } from '@/models/utils/stringBuilder';
export default Vue.extend({
  data() {
    return {
      rows: [] as PensionRow[],
      showDeclinePopup: false,
      showApprovePopup: false,
      selectedRow: '',
      newSolution: {
        destination: formatDate(new Date().toISOString()),
        execution: formatDate(new Date().toISOString()),
        comment: '',
      },
      requestResult: {
        success: true,
        message: '',
        showPopup: false,
      },
    };
  },
  mounted() {
    this.getPensionUpdateRows();
  },
  computed: {
    requestSender(): RequestSender {
      return this.$store.getters.requestSender;
    },
    selectedRowsIds(): string[] {
      return this.selectedRow
        ? [this.selectedRow]
        : this.rows
          .filter((el) => el.selected)
          .map((el) => el.id);
    },
  },
  methods: {
    selectAllRows() {
      this.rows.forEach((el) => el.selected = true);
    },
    selectNoRows() {
      this.rows.forEach((el) => el.selected = false);
    },
    closeApprovePopup() {
      this.selectedRow = '';
      this.showApprovePopup = false;
    },
    closeDeclinePopup() {
      this.selectedRow = '';
      this.showDeclinePopup = false;
    },
    confirmApproveOne(id: string) {
      this.selectedRow = id;
      this.showApprovePopup = true;
    },
    confirmDeclineOne(id: string) {
      this.selectedRow = id;
      this.showDeclinePopup = true;
    },
    confirmApproveSelected() {
      this.showApprovePopup = true;
    },
    confirmDeclineSelected() {
      this.showDeclinePopup = true;
    },
    syncAllGosPension() {
      this.requestSender.syncAllGosPension()
        .then(() => {
          this.getPensionUpdateRows();
        })
        .catch((resp) => {
          this.requestResult.success = false;
          this.requestResult.message = 'Не удалось получить данные из ПФР. Попробуйте позже.';
          this.requestResult.showPopup = true;
        });
    },
    getPensionUpdateRows() {
      this.requestSender.getPensionUpdateRows()
        .then((resp) => {
          this.rows = resp.data.map((el: any) => new PensionRow(el));
        })
        .catch((resp) => {
          this.requestResult.success = false;
          this.requestResult.message = resp.data;
          this.requestResult.showPopup = true;
        });
    },
    approveRows() {
      this.requestSender.approveGosPension(
        this.selectedRowsIds,
        this.newSolution.destination,
        this.newSolution.execution,
        this.newSolution.comment,
      ).then(() => {
        this.removeRows(this.selectedRowsIds);
      }).finally(() => {
        this.showApprovePopup = false;
      });
    },
    declineRows() {
      this.requestSender.declineGosPension(this.selectedRowsIds)
        .then(() => {
          this.removeRows(this.selectedRowsIds);
        }).finally(() => {
          this.showDeclinePopup = false;
        });
    },
    removeRows(ids: string[]) {
      this.rows = this.rows.filter((el) => !ids.includes(el.id));
    },
  },
  components: {
    CommonButton,
    SaveButton,
    LabeledInput,
    Scrollable,
    Popup,
    RequestResultPopup,
  },
});
</script>
