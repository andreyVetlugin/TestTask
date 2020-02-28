<template>
  <Popup class="solution_print_popup" :shown="shown" @close="$emit('close')" title="Справки о решениях">
    <PopupLinkedRow v-for="(link, i) in links" :key="i" :filename="link.filename" :link="link.link"></PopupLinkedRow>
  </Popup>
</template>

<style>
.solution_print_popup .popup-content {
  display: flex;
  flex-direction: column;
}
.solution_print_popup .popup-content .popup_linked_row {
  width: 100%;
}
</style>


<script lang="ts">
import Vue from 'vue';
import Popup from '@/components/Popup.vue';
import PopupLinkedRow from '@/components/CardView/Popups/PopupLinkedRow.vue';
export default Vue.extend({
  data() {
    return {
      links: [
        // {
        //   filename: 'Решение о возобновлении выплат',
        //   link: 'PrintResumeSolution',
        // },
        // {
        //   filename: 'Решение о прекращении выплаты пенсии за выслугу лет',
        //   link: 'PrintStopSolution',
        // },
        // {
        //   filename: 'Решение о приостановлении выплат',
        //   link: 'PrintPauseSolution',
        // },
        // {
        //   filename: 'Решение об определении',
        //   link: 'PrintOpredelitSolution',
        // },
        {
          filename: 'Решение об отказе в перерасчете размера пенсии за выслугу лет',
          link: 'PrintRejectSolution',
        },
      ],
    };
  },
  props: {
    shown: Boolean,
  },
  components: {
    Popup,
    PopupLinkedRow,
  },
});
</script>
