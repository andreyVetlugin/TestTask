<template>
  <Popup class="work_popup" :shown="shown" :title="title" @close="$emit('close')">
    <LabeledInput
      required
      pattern="date"
      placeholder="дд.мм.гггг"
      v-model="editWork.startDate"
      v-on:validated="validateStartDate"
    ></LabeledInput>
    <h4>—</h4>
    <LabeledInput
      required
      pattern="date"
      placeholder="дд.мм.гггг"
      v-model="editWork.endDate"
      v-on:validated="validateEndDate"
    ></LabeledInput>
    <LabeledInputWithPrompt
      title="Организация"
      v-model="editWork.organizationName"
      :prompts="organizationsList"
    ></LabeledInputWithPrompt>
    <LabeledInputWithPrompt title="Должность" v-model="editWork.function" :prompts="functionsList"></LabeledInputWithPrompt>
    <CommonButton @click="$emit('click')" :disabled="!canSend" :text="'Сохранить'"></CommonButton>
  </Popup>
</template>

<style>
.work_popup h4 {
  margin-left: 8px;
  margin-right: 8px;
  width: 16px;
  text-align: center;
}
.work_popup .prompt_input_container {
  margin-right: 32px;
}
.work_popup .prompt_input_container:last-of-type {
  margin-right: 0;
}
</style>

<script lang="ts">
import Vue from 'vue';
import moment from 'moment';
import Popup from '@/components/Popup.vue';
import CommonButton from '@/components/CommonButton.vue';
import LabeledInput from '@/components/LabeledInput.vue';
import LabeledInputWithPrompt from '@/components/LabeledInputWithPrompt.vue';
import DropDown from '@/components/DropDown.vue';
import WorkInfo from '@/models/person/workInfo';
export default Vue.extend({
  data() {
    return {
      startDateValid: true,
      endDateValid: true,
    };
  },
  props: {
    editWork: WorkInfo,
    shown: Boolean,
    title: String,
  },
  computed: {
    functionsList(): Array<{}> {
      return this.$store.getters.functionsList.map((el: any) => {
        return {
          value: el.id,
          title: el.name,
        };
      });
    },
    organizationsList(): Array<{}> {
      return this.$store.getters.organizationsList.map((el: any) => {
        return {
          value: el.id,
          title: el.organizationName,
        };
      });
    },
    canSend(): boolean {
      return this.startDateValid &&
        this.endDateValid &&
        moment(this.editWork.startDate, 'DD.MM.YYYY').isBefore(moment(this.editWork.endDate, 'DD.MM.YYYY')) &&
        !!this.editWork.organizationName &&
        !!this.editWork.function;
    },
  },
  methods: {
    validateStartDate(val: boolean) {
      this.startDateValid = val;
    },
    validateEndDate(val: boolean) {
      this.endDateValid = val;
    },
  },
  components: {
    Popup,
    CommonButton,
    LabeledInput,
    LabeledInputWithPrompt,
  },
});
</script>
