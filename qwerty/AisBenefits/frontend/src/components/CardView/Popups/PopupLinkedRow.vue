<template>
  <div class="popup_linked_row">
    <h4>{{filename}}</h4>
    <CommonButton noBackground @click="download" text="скачать"></CommonButton>
  </div>
</template>

<style>
.popup_linked_row {
  display: flex;
  flex-direction: row;
  justify-content: space-between;
  align-items: center;
}
.popup_linked_row .common-button {
  text-decoration: underline;
}
</style>


<script lang="ts">
import Vue from 'vue';
import moment from 'moment';
import CommonButton from '@/components/CommonButton.vue';
import RequestSender from '@/requestSender';
export default Vue.extend({
  props: {
    link: String,
    filename: String,
  },
  computed: {
    requestSender(): RequestSender {
      return this.$store.getters.requestSender;
    },
    selectedCardId(): string {
      return this.$store.getters.selectedCardId;
    },
    selectedCardFio(): string {
      let result = '';
      const card = this.$store.getters.selectedCard;
      if (card) {
        if (card.firstName && card.firstName.length) {
          result += card.firstName + ' ';
        }
        if (card.secondName && card.secondName.length) {
          result += card.secondName[0] + '.';
        }
        if (card.surName && card.surName.length) {
          result += card.surName[0] + '.';
        }
      }
      return result;
    },
  },
  methods: {
    download() {
      const fname = `${this.filename} ${this.selectedCardFio} от ${moment().locale('ru').format('DD.MM.YYYY')}`;
      this.requestSender.printSolutionCertificate(this.link, this.selectedCardId, this.filename);
    },
  },
  components: {
    CommonButton,
  },
});
</script>
