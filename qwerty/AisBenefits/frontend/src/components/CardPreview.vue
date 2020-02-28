<template>
  <div class="card-preview" :class="{active: isSelected, semitransparent: paused}" @click="selectCard" @mousewheel="emit">
    <h1>{{number}}</h1>
    <h2 class="text-align-right">
      <h4 v-if="employeeType">{{employeeType}}</h4>
      <h4 v-if="payoutType">{{payoutType}}</h4>
      <h4 v-if="variant">{{variant}}</h4>
    </h2>
    <h2>{{fio}} <span style="color: #503f96; margin: 0 8px;">|</span> {{birthDate}}</h2>
    <!-- <h5 class="h5-with-delimeter"></h5> -->
    <h5 class="h5-with-delimeter" v-if="!isSelected">
      <i class="fa" :class=" stageApproved ? 'fa-check' : 'warning_triangle' "></i>
      <span style="line-height: normal">стаж{{ stageApproved ? ' ' : ' не ' }}подтверждён</span>
    </h5>
    <template v-if="isSelected">
      <template v-if="additionalInfo.org">
        <h5>Организация</h5>
        <h4>{{additionalInfo.org}}</h4>
      </template>
      <template v-if="additionalInfo.func">
        <h5>Должность</h5>
        <h4>{{additionalInfo.func}}</h4>
      </template>
      <template v-if="additionalInfo.start">
        <h5>Назначение</h5>
        <h4>{{additionalInfo.start}}</h4>
      </template>
      <template v-if="additionalInfo.end">
        <h5>Увольнение</h5>
        <h4>{{additionalInfo.end}}</h4>
      </template>
      <template v-if="additionalInfo.worksAge">
        <h5>Стаж</h5>
        <h4>
          {{additionalInfo.worksAge}}
          <i
            class="fa"
            :class=" additionalInfo.approved ? 'fa-check' : 'warning_triangle' "
          ></i>
        </h4>
      </template>
    </template>
  </div>
</template>

<style>
.card-preview {
  background-color: #3f3176;
  border: 1px solid #493c80;
  border-radius: 4px;
  box-sizing: border-box;
  padding: 16px 24px;
  font-size: 0;
}
.card-preview:hover {
  background-color: #493c80;
  cursor: pointer;
}
.card-preview.active {
  background-color: #503f96;
  border: 1px solid #3a3066;
}
.card-preview h1 {
  margin: 0;
  display: inline-block;
  font-size: 24pt;
}
.card-preview h2 {
  margin: 8px 0 0;
  font-size: 14pt;
  font-family: "Rubik-Light";
}
.card-preview .text-align-right {
  width: 80px;
  float: right;
  text-align: right;
}
h2.text-align-right h4 {
  margin: 0 0 8px;
  display: inline-block;
  font-size: 0.8em;
  width: 100%;
}
.h5-with-delimeter {
  margin: 24px 0 0;
  display: flex;
  align-items: flex-start;
  font-size: 10pt;
}
.card-preview .h5-with-delimeter .fa {
  margin: 0 8px 0 0;
}
.card-preview .h5-with-delimeter .fa-check {
  margin-top: -4px;
}
.delimeter {
  color: #d49e35;
  font-size: 6px;
  margin: 0 10px;
}
.card-preview .fa {
  font-size: 16pt;
  margin: 0 16px;
}
.card-preview .fa-check {
  color: greenyellow;
}
.card-preview .warning_triangle {
  width: 16px;
  height: 16px;
  background: url("/images/warning_triangle_16.png") no-repeat;
}

.card-preview.semitransparent {
    background-color: rgba(63, 49, 118, 0.25);
    border: 1px solid rgba(73, 60, 128, 0.25);
}
.card-preview.semitransparent h1,
.card-preview.semitransparent h2,
.card-preview.semitransparent h4,
.card-preview.semitransparent h5 {
    color: rgba(225, 225, 225, 0.5);
}
.card-preview.semitransparent .fa-check {
    color: rgb(173, 255, 47, 0.25);
}
</style>

<script lang="ts">
import Vue from 'vue';
import PersonInfo from '@/models/person/personInfo';
import WorkInfo from '@/models/person/workInfo';
import { formatDate } from '@/models/utils/stringBuilder';
export default Vue.extend({
  props: {
    id: String,
    number: String,
    surName: String,
    firstName: String,
    secondName: String,
    birthDate: String,
    address: String,
    phone: String,
    employeeType: String,
    payoutType: String,
    variant: String,
    stageApproved: Boolean,
    paused: Boolean,
  },
  computed: {
    fio(): string {
      let result = this.surName + ' ';
      if (this.firstName) {
        result += this.firstName[0] + '.';
      }
      if (this.secondName) {
        result += this.secondName[0] + '.';
      }
      return result.trim();
    },
    isSelected(): boolean {
      return this.id === this.$store.getters.selectedCardId;
    },
    additionalInfo(): any {
      const card = this.$store.getters.selectedCard as PersonInfo;
      const lastWork = card && card.works ? card.works[0] : new WorkInfo({});
      return lastWork
        ? {
          org: lastWork.organizationName,
          func: lastWork.function,
          start: card.docsDestinationDate,
          end: lastWork.endDate,
          worksAge: card.worksAge,
          approved: card.worksApproved,
        }
        : {
          org: '',
          func: '',
          start: card.docsDestinationDate,
          end: '',
          worksAge: '',
          approved: card.worksApproved,
        };
    },
  },
  methods: {
    selectCard(): void {
      this.$store.commit('setSelectedCardId', this.id);
      const router = this.$router;
      let path = router.currentRoute.fullPath + '';
      path = path.indexOf('?') >= 0
        ? path.indexOf('cardId') >= 0
          ? path.replace(/cardId=[^&]*/, 'cardId=' + this.id)
          : path + '&cardId=' + this.id
        : path + '?cardId=' + this.id;
      setTimeout(() => {
        router.push(path);
      }, 0);
      this.$emit('click');
    },
    formatDate(date: string): string {
      return formatDate(date);
    },
    emit(e: any) {
      if (e && e.type) {
        this.$emit(e.type, e);
      }
    },
  },
});
</script>
