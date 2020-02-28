<template>
  <div class="page_layout zags">
    <div class="zags-view shadow_medium">
      <h1>Выгрузка из ЗАГС</h1>
      <div class="zags_file_load">
        <label for="file_input" class="common-button">Загрузить файл</label>
        <input
          id="file_input"
          style="display: none;"
          type="file"
          accept=".xls, .xlsx"
          @change="loadFile"
        />
      </div>
      <template v-if="fileSended">
        <h2 v-if="!deadmens || deadmens.length === 0">Соответствий не найдено</h2>
        <Scrollable v-else>
          <div class="zags_row">
            <h5>№ карты</h5>
            <h5>ФИО</h5>
            <h5>Дата смерти</h5>
            <div class="button_block"></div>
          </div>
          <div class="zags_row" v-for="dead of deadmens" :key="dead.id">
            <h4>{{dead.number}}</h4>
            <h4>{{dead.fio}}</h4>
            <h4>{{dead.deadend}}</h4>
            <div class="button_block">
              <CommonButton text="Подтвердить" @click="showPopup(dead.id)"></CommonButton>
              <CommonButton text="Отклонить" @click="setUndead(dead.id)"></CommonButton>
            </div>
          </div>
        </Scrollable>
      </template>
    </div>
    <Popup :shown="showSolutionPopup" title="Прекратить" @close="showSolutionPopup = false">
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
      <LabeledInput
        class="width-240"
        v-model="newSolution.comment"
        placeholder="комментарий"
      ></LabeledInput>
      <div class="popup_br"></div>
      <CommonButton
        @click="sendSolution"
        :disabled="!newSolution.destination || !newSolution.execution"
        text="Отправить"
      ></CommonButton>
    </Popup>
    <div class="command_menu"></div>
  </div>
</template>

<style>
.zags-view {
  display: flex;
  height: 100%;
  border-radius: 4px;
  background-color: #6254a3;
  box-sizing: border-box;
  flex-wrap: nowrap;
  flex: 1 1 100%;
  flex-direction: column;
  margin: 0 0 0 16px;
  padding: 0 48px;
}
.zags_file_load {
  padding: 8px 0;
  flex: 0 0 64px;
  margin: 24px 0 16px;
}
.zags_file_load .common-button {
  width: 240px;
  text-align: center;
  line-height: 1em;
}

.zags_row {
  display: flex;
  flex-direction: row;
  padding: 8px 0;
  flex: 0 0 64px;
  align-items: center;
  justify-content: space-between;
  flex-wrap: nowrap;
  border-bottom: 1px solid #534590;
}
.zags_row .button_block {
  width: 336px;
  display: flex;
  justify-content: space-between;
}
.zags_row h4,
.zags_row h5 {
  width: calc((100% - 384px) / 3);
}
</style>

<script lang="ts">
import Vue from 'vue';
import LabeledInput from '@/components/LabeledInput.vue';
import CommonButton from '@/components/CommonButton.vue';
import Scrollable from '@/components/Scrollable.vue';
import Popup from '@/components/Popup.vue';
import { formatDate } from '@/models/utils/stringBuilder';
import axios, { AxiosError, AxiosResponse } from 'axios';
import RequestSender from '@/requestSender';
import Axios from 'axios';
export default Vue.extend({
  data() {
    return {
      showSolutionPopup: false,
      selectedCardId: '',
      deadmens: [] as Array<{ id: string, number: string, fio: string, deadend: string }>,
      fileSended: false,
      newSolution: {
        id: '',
        destination: formatDate(new Date().toISOString()),
        execution: formatDate(new Date().toISOString()),
        comment: '',
      },
    };
  },
  mounted() {
    // todo load config;
    axios.get('/api/zags/acts')
      .then((resp: AxiosResponse) => {
        if (resp.data && resp.data.length) {
          this.fileSended = true;
          this.deadmens = resp.data.map((el: any) => ({
            id: el.id,
            number: el.number,
            fio: el.fio,
            deadend: formatDate(el.deathDate),
          }));
        }
      });
  },
  computed: {
    requestSender(): RequestSender {
      return this.$store.getters.requestSender;
    },
  },
  watch: {
    showSolutionPopup(val) {
      if (!val) {
        this.newSolution = {
          id: '',
          destination: formatDate(new Date().toISOString()),
          execution: formatDate(new Date().toISOString()),
          comment: '',
        };
      }
    },
  },
  methods: {
    loadFile(e: any) {
      if (!e.target || !e.target.files || !e.target.files.length) {
        this.fileSended = false;
        return;
      }
      const formData = new FormData();
      formData.append('file', e.target.files[0]);
      this.requestSender.loadDeadmenFile(formData)
        .then((resp: AxiosResponse) => {
          this.fileSended = true;
          if (resp.data && resp.data.length) {
            this.deadmens = resp.data.map((el: any) => ({
              id: el.id,
              number: el.number,
              fio: el.fio,
              deadend: formatDate(el.deathDate),
            }));
          }
        });
    },
    showPopup(id: string) {
      this.newSolution.id = id;
      this.showSolutionPopup = true;
    },
    sendSolution() {
      axios.post('/api/zags/approveact', {
        id: this.newSolution.id,
        destinationDate: this.newSolution.destination,
        executionDate: this.newSolution.execution,
        comment: this.newSolution.comment,
      }).then(() => {
        this.deadmens = this.deadmens.filter((el) => el.id !== this.newSolution.id);
        this.showSolutionPopup = false;
      });
    },
    setUndead(id: string) {
      axios.post('/api/zags/declineact', {
        id,
      });
      this.deadmens = this.deadmens.filter((el) => el.id !== id);
    },
  },
  components: {
    LabeledInput,
    CommonButton,
    Scrollable,
    Popup,
  },
});
</script>
