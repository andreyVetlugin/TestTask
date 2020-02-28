<template>
  <div class="page_layout mass_recount_layout">
    <div class="mass_recount shadow_medium">
      <h1>Массовые перерасчёты</h1>
      <div class="recount_block">
        <h5>Тип перерасчёта</h5>
        <h4 class="pension_checkbox" :class="{ selected: pension }" @click="pension = true">
          <i class="fa fa-check"></i>Госпенсия
        </h4>
        <h4
          class="pension_checkbox"
          :class="{ selected: pension === false }"
          @click="pension = false"
        >
          <i class="fa fa-check"></i>Должностной оклад
        </h4>
      </div>
      <div v-if="pension !== undefined" class="recount_block">
        <template v-if="pension">
          <h5>Тип выплат</h5>
          <h4
            class="pension_checkbox"
            v-for="item in pensions"
            :class="{ selected: item.selected }"
            @click="item.selected = !item.selected"
            :key="item.value"
          >
            <i class="fa fa-check"></i>
            {{item.title}}
          </h4>
        </template>
        <template v-else>
          <h4
            class="pension_checkbox"
            style="width: 100%; flex: 0 0 100%;"
            :class="{ selected: isAll === true }"
            @click="isAll = !isAll"
          >
            <i class="fa fa-check"></i>Пересчитать всем
          </h4>
          <br />
          <div class="list_container">
            <DropDown editable title="Добавить вариант рассчёта" :list="extraPayVariants" :disabled="isAll" @input="addVariant"></DropDown>
            <h5 v-show="false && selectedVariants.length">Выбранные варианты рассчёта</h5>
            <Scrollable hideScroll v-show="!isAll">
              <h4 class="recount_item" v-for="item in selectedVariants" :key="item.value">
                <CommonButton
                  noBackground
                  class="delete-button"
                  html="<i class='fa fa-times'></i>"
                  @click="removeVariant(item.value)"
                ></CommonButton>
                {{item.title}}
              </h4>
            </Scrollable>
          </div>
          <div class="list_container">
            <DropDown editable title="Добавить должность" :list="functions" :disabled="isAll" @input="addItem"></DropDown>
            <h5 v-show="false && selectedFunctions.length">Выбранные должности</h5>
            <Scrollable hideScroll v-show="!isAll">
              <h4 class="recount_item" v-for="item in selectedFunctions" :key="item.value">
                <CommonButton
                  noBackground
                  class="delete-button"
                  html="<i class='fa fa-times'></i>"
                  @click="removeItem(item.value)"
                ></CommonButton>
                {{item.title}}
              </h4>
            </Scrollable>
          </div>
        </template>
      </div>
      <div v-if="isAll || ids.length || variantIds.length" class="recount_block">
        <LabeledInput pattern="9.9999" noclear title="Коэффициент" v-model="koef"></LabeledInput>
        <LabeledInput
          pattern="99.99.9999"
          placeholder="дд.мм.гггг"
          title="Дата назначения"
          v-model="destination"
        ></LabeledInput>
        <LabeledInput
          pattern="99.99.9999"
          placeholder="дд.мм.гггг"
          title="Дата исполнения"
          v-model="execution"
        ></LabeledInput>
      </div>
      <div v-if="isAll || ids.length || variantIds.length" class="recount_block">
        <CommonButton @click="showPopup = true" text="Пересчитать"></CommonButton>
      </div>
    </div>
    <Popup
      class="mass_recount_popup"
      :shown="showPopup"
      title="Подтверждение"
      @close="showPopup = false"
    >
      <template>
        <h4>
          Вы уверены что хотите выполнить массовый перерасчёт?
          <br />Данное действие является необратимым.
        </h4>
        <div class="popup_br"></div>
        <CommonButton @click="sendRecount" text="Выполнить"></CommonButton>
        <CommonButton @click="showPopup = false" text="Отмена"></CommonButton>
      </template>
    </Popup>

    <RequestResultPopup @close="requestResult.showPopup = false" :requestResult="requestResult"></RequestResultPopup>

    <div class="command_menu">
      <SaveButton v-if="ids.length || variantIds.length" @click="showPopup = true"></SaveButton>
    </div>
  </div>
</template>

<style>
.mass_recount {
  background-color: #493c80;
  border-radius: 4px;
  display: flex;
  flex-wrap: nowrap;
  justify-content: flex-start;
  align-items: space-between;
  margin: 0 0 0 16px;
  padding: 0 48px;
  flex: 1 1 100%;
  flex-direction: column;
}
.recount_block {
  flex: 0 0 auto;
  display: flex;
  justify-content: flex-start;
  align-items: flex-start;
  flex-wrap: wrap;
  margin-bottom: 24px;
  max-height: calc(100% - 520px);
}
.recount_block.flexible {
  /* flex-direction: column; */
  flex-wrap: nowrap;
}
.recount_block > * {
  margin: 12px 24px 12px 0;
}
.recount_block > h5 {
  flex: 0 0 100%;
}
.recount_block .scrollable-container {
  margin: 0;
}
.recount_block .dropdown,
.recount_block > .labeled-input,
.recount_block > .common-button {
  width: 240px;
}
.recount_block > .dropdown ul {
  margin: 8px -40% 0 0;
  width: 140%;
}
.recount_block > .common-button {
  margin: 0;
}

.list_container {
    display: flex;
    width: 50%;
    flex-direction: column;
    -ms-flex-wrap: nowrap;
    flex-wrap: nowrap;
    margin-right: 0;
}

.pension_checkbox {
  display: inline-block;
  width: 240px;
  /* margin-bottom: 24px; */
  flex: 0 0 240px;
  display: flex;
  align-items: center;
}
.pension_checkbox .fa {
  color: transparent;
  font-size: 10px;
  padding: 4px;
  margin: 2px 8px 2px 0;
  height: 18px;
  width: 18px;
  border-radius: 50%;
  background-color: #503f96;
  box-sizing: border-box;
}
.pension_checkbox:hover .fa {
  background-color: #8a7ec0;
}
.pension_checkbox:active .fa {
  background-color: #6253a2;
  color: #ffffff;
}
.pension_checkbox.selected .fa {
  color: #ffffff;
}

.recount_item {
  display: flex;
  align-items: center;
  height: 16pt;
}
.recount_item > * {
  padding: 0;
}

.mass_recount_popup .popup-content {
  display: flex;
  flex-wrap: wrap;
  justify-content: center;
}
.mass_recount_popup h4 {
  text-align: center;
  margin: 0 24px 24px;
}

.mass_recount_popup .popup-content .common-button {
  margin-right: 32px;
  flex: 0 0 160px;
}
.mass_recount_popup .popup-content .common-button:last-of-type {
  margin-right: 0;
}
</style>

<script lang="ts">
import Vue from 'vue';
import Scrollable from '@/components/Scrollable.vue';
import CommonButton from '@/components/CommonButton.vue';
import SaveButton from '@/components/Common/Buttons/SaveButton.vue';
import DropDown from '@/components/DropDown.vue';
import Popup from '@/components/Popup.vue';
import RequestResultPopup from '@/components/Common/Popups/RequestResultPopup.vue';
import LabeledInput from '@/components/LabeledInput.vue';
import RequestSender from '@/requestSender';
import { formatDate } from '@/models/utils/stringBuilder';
export default Vue.extend({
  data() {
    return {
      pension: undefined as (boolean | undefined),
      isAll: false,
      selectedFunctions: [] as Array<{}>,
      selectedVariants: [] as Array<{}>,
      pensions: [] as Array<{}>,
      koef: '1.00',
      destination: formatDate(new Date().toISOString()),
      execution: formatDate(new Date().toISOString()),
      showPopup: false,
      requestResult: {
        success: true,
        message: '',
        showPopup: false,
      },
    };
  },
  mounted() {
    this.requestSender.getAllFunctions();
    this.requestSender.getAllExtraPayVariants();
    this.requestSender.loadCardEditData();
  },
  computed: {
    selectedPensions(): Array<{}> {
      return this.pension ? this.selectedPensions : this.selectedFunctions;
    },
    requestSender(): RequestSender {
      return this.$store.getters.requestSender;
    },
    functions(): Array<{}> {
      return this.$store.getters.functionsList.map((el: any) => ({ value: el.id, title: el.name })) || [];
    },
    payoutTypes(): Array<{}> {
      return this.$store.getters.cardEditData.payoutTypes || [];
    },
    extraPayVariants(): Array<{}> {
      return this.$store.getters.extraPayVariants
        .map((el: any) => ({ value: el.id, title: el.number.toString() })) || [];
    },
    ids(): string[] {
      return this.pension
        ? this.pensions.filter((el: any) => el.selected).map((el: any) => el.value)
        : this.pension !== undefined
          ? this.selectedFunctions.map((el: any) => el.value)
          : [];
    },
    variantIds(): string[] {
      return this.selectedVariants.map((el: any) => el.value);
    },
  },
  watch: {
    payoutTypes(val) {
      this.pensions = val.map((el: any) => {
        return {
          value: el.value,
          title: el.title,
          selected: false,
        };
      });
    },
    pension() {
      this.selectedFunctions = [] as Array<{}>;
      this.selectedVariants = [] as Array<{}>;
      this.pensions = [] as Array<{}>;
    },
  },
  methods: {
    addItem(id: string) {
      if (this.selectedFunctions.some((el: any) => el.value === id)) {
        return;
      }
      const item = (this.functions).find((el: any) => el.value === id);
      if (item) {
        this.selectedFunctions.push(item);
      }
    },
    addVariant(id: string) {
      if (this.selectedVariants.some((el: any) => el.value === id)) {
        return;
      }
      const item = (this.extraPayVariants).find((el: any) => el.value === id);
      if (item) {
        this.selectedVariants.push(item);
      }
    },
    removeItem(id: string) {
      this.selectedFunctions = this.selectedFunctions.filter((el: any) => el.value !== id);
    },
    removeVariant(id: string) {
      this.selectedVariants = this.selectedVariants.filter((el: any) => el.value !== id);
    },
    sendRecount() {
      const sendData = {
        isPension: this.pension,
        isAll: this.isAll,
        ids: this.ids,
        variantIds: this.variantIds,
        koef: parseFloat(this.koef),
        destination: this.destination,
        execution: this.execution,
      };
      this.requestSender.massRecount(sendData)
        .then(() => {
          this.requestResult.success = true;
          this.requestResult.message = 'Перерасчёт успешно применён';
          this.requestResult.showPopup = true;
          this.showPopup = false;
        })
        .catch((resp) => {
          this.requestResult.success = false;
          this.requestResult.message = resp.response.data.message;
          this.requestResult.showPopup = true;
          this.showPopup = false;
        });
    },
  },
  components: {
    LabeledInput,
    DropDown,
    CommonButton,
    SaveButton,
    Scrollable,
    Popup,
    RequestResultPopup,
  },
});
</script>
