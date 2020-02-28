<template>
  <div class="page_layout edit_layout">
    <div class="card-edit shadow_medium">
      <div class="tab-navigation">
        <CommonButton
          :class="{ selected: selectedTab === 'main_info'}"
          @click="selectedTab = 'main_info'"
          text="Карта"
        ></CommonButton>
        <CommonButton
          :class="{ selected: selectedTab === 'work_info'}"
          @click="selectedTab = 'work_info'"
          :disabled="!selectedCardId"
          text="Стаж"
        ></CommonButton>
        <CommonButton
          :class="{ selected: selectedTab === 'extra_pay'}"
          @click="selectedTab = 'extra_pay'"
          :disabled="!selectedCardId"
          text="Расчёт доплат"
        ></CommonButton>
        <CommonButton
          :class="{ selected: selectedTab === 'solution'}"
          @click="selectedTab = 'solution'"
          :disabled="!selectedCardId"
          text="Решения"
        ></CommonButton>
        <CommonButton
          :class="{ selected: selectedTab === 'payout'}"
          @click="selectedTab = 'payout'"
          :disabled="!selectedCardId"
          text="Выплаты"
        ></CommonButton>
      </div>
      <div v-show="selectedTab === 'main_info'" class="tab-content main-info_tab">
        <Scrollable hideScroll>
          <LabeledInput title="Регистрационный номер" v-model="card.number" required></LabeledInput>
          <DropDown
            required
            class="double"
            title="Тип получателя"
            v-model="card.employeeTypeId"
            :list="cardEditData.employmentTypes"
          ></DropDown>
          <DropDown
            required
            title="Тип выплаты"
            v-model="card.payoutTypeId"
            :list="cardEditData.payoutTypes"
          ></DropDown>
          <DropDown
            required
            disabled
            title="Вариант расчёта"
            v-model="card.extraPay.variantId"
            :list="extraPayVariants"
          ></DropDown>

          <LabeledInput
            :deniedReg="/[^а-яА-ЯёЁ\ -]/g"
            title="Фамилия"
            v-model="card.surName"
            required
          ></LabeledInput>
          <LabeledInput
            :deniedReg="/[^а-яА-ЯёЁ\ -]/g"
            title="Имя"
            v-model="card.firstName"
            required
          ></LabeledInput>
          <LabeledInput :deniedReg="/[^а-яА-ЯёЁ\ -]/g" title="Отчество" v-model="card.secondName"></LabeledInput>
          <DropDown required title="Пол" v-model="card.sex" :list="cardEditData.sexList"></DropDown>
          <LabeledInput title="СНИЛС" pattern="snils" v-model="card.snils"></LabeledInput>

          <LabeledInput
            required
            pattern="date"
            placeholder="дд.мм.гггг"
            title="Дата рождения"
            v-model="card.birthDate"
          ></LabeledInput>
          <LabeledInput
            :deniedReg="/[^а-яА-ЯёЁ\-\d,./ ]/g"
            title="Место рождения"
            v-model="card.birthPlace"
          ></LabeledInput>
          <LabeledInput
            :deniedReg="/[^а-яА-ЯёЁ\-\d,./ ]/g"
            class="double"
            title="Адрес"
            :maxlength="256"
            v-model="card.address"
          ></LabeledInput>
          <!-- x2 -->
          <LabeledInput title="Телефон" pattern="+7 (999) 999-99-99" v-model="card.phone"></LabeledInput>

          <LabeledInput
            title="Серия документа"
            type="number"
            :maxlength="(card.documentTypeId === '03') ? 4 : 10"
            v-model="card.documentSeria"
          ></LabeledInput>
          <LabeledInput
            title="Номер документа"
            type="number"
            :maxlength="card.documentTypeId === '03' ? 6 : 15"
            v-model="card.documentNumber"
          ></LabeledInput>
          <LabeledInputWithPrompt
            class="double"
            title="Кем выдан"
            v-model="card.documentIssuer"
            :prompts="cardEditData.documentIssuers"
            :deniedReg="/[^а-яА-ЯёЁ\-\d,./ ]/g"
          ></LabeledInputWithPrompt>
          <!-- x2 -->
          <LabeledInput
            pattern="date"
            placeholder="дд.мм.гггг"
            title="Когда выдан"
            v-model="card.documentIssueDate"
          ></LabeledInput>

          <DropDown
            required
            title="Тип пенсии"
            v-model="card.pensionTypeId"
            :list="cardEditData.pensionTypes"
            dropup
          ></DropDown>
          <LabeledInput
            pattern="date"
            placeholder="дд.мм.гггг"
            title="Срок выплаты по.."
            v-model="card.pensionEndDate"
            :disabled="card.pensionTypeId !== 'd666a556-46f1-4b82-a242-ba9c36a0cae0'"
          ></LabeledInput>
          <DropDown
            class="double"
            title="Район получения пенсии"
            v-model="card.districtId"
            :list="cardEditData.districts"
            editable
            dropup
          ></DropDown>
          <DropDown
            required
            title="Дополнительная пенсия"
            v-model="card.additionalPensionId"
            :list="cardEditData.additionalPensionTypes"
            dropup
          ></DropDown>
          <!--  количество должно быть кратно пяти -->
        </Scrollable>
      </div>

      <div v-show="selectedTab === 'work_info'" class="tab-content work-info_tab">
        <div class="work-info_row">
          <CommonButton noBackground></CommonButton>
          <h5>Период</h5>
          <h5>Стаж</h5>
          <h5>Организация</h5>
          <h5>Должность</h5>
          <CommonButton noBackground></CommonButton>
        </div>
        <Scrollable>
          <div class="work-info_row" v-for="work in workInfos" :key="work.id">
            <EditButton noBackground @click="editWorkInfo(work)"></EditButton>
            <h4 v-text="'с ' + (work.startDate) + ' по ' + ((work.endDate) || 'настоящее время')"></h4>
            <h4 v-text="work.age"></h4>
            <h4 v-text="work.organizationName"></h4>
            <h4 v-text="work.function"></h4>
            <CommonButton
              noBackground
              class="delete-button"
              html="<i class='fa fa-times'></i>"
              @click="deleteWorkInfo(work)"
            ></CommonButton>
          </div>
        </Scrollable>
        <div class="work-info_total-row">
          <LabeledBlock lined>
            <template slot="title">
              <h2>Даты</h2>
            </template>
            <template slot="content">
              <div v-if="workEndDate">
                <template v-show="workEndDate">
                  <h5>Увольнения</h5>
                  <h4>{{workEndDate}}</h4>
                </template>
              </div>
              <div>
                <template v-show="selectedCard.docsDestinationDate">
                  <h5>Назначения</h5>
                  <h4>{{selectedCard.docsDestinationDate}}</h4>
                </template>
              </div>
              <div>
                <template v-show="selectedCard.docsSubmitDate">
                  <h5>Подачи документов</h5>
                  <h4>{{selectedCard.docsSubmitDate}}</h4>
                </template>
              </div>
            </template>
          </LabeledBlock>
          <h2>Общий стаж</h2>
          <h3>{{selectedCard.worksAge}}</h3>
        </div>
        <div class="stage_block">
          <h3
            class="stage-approvement"
            v-show="workInfos && workInfos.length"
          >Стаж{{ card.worksApproved ? ' ' : ' не ' }}подтвержден</h3>
          <CommonButton
            v-show="workInfos && workInfos.length"
            @click="showWorkApprovePopup = true"
            text="Утверждение стажа"
          ></CommonButton>
        </div>
      </div>

      <div v-show="selectedTab === 'extra_pay'" class="tab-content extra_pay_tab">
        <Scrollable hideScroll>
          <LabeledBlock lined>
            <template slot="title">
              <h2>Начисления</h2>
            </template>
            <template slot="content">
              <DropDown title="Вариант расчёта" v-model="variantId" :list="extraPayVariants"></DropDown>
              <LabeledInput pattern="money" title="Уральский к-т" v-model="uralMultiplier"></LabeledInput>
              <LabeledInput pattern="money" title="Оклад" v-model="salary" @blur="formatInputMoney"></LabeledInput>
              <LabeledInput disabled title="Оклад (с ур. коэф.)" v-model="salaryMultiplied"></LabeledInput>
              <div class="input_block">
                <LabeledInput
                  pattern="money"
                  title="Премия"
                  v-model="premium"
                  @change="calcAllPercsBySumm"
                  @blur="formatInputMoney"
                ></LabeledInput>
                <LabeledInput
                  :deniedReg="/[^\d]/g"
                  title="Процент"
                  @change="calcAllSummByPercs"
                  v-model="premiumPerc"
                ></LabeledInput>
              </div>
              <LabeledInput disabled title="Мат. помощь" v-model="materialSupport"></LabeledInput>
              <div class="input_block">
                <LabeledInput
                  pattern="money"
                  title="Надбавки"
                  v-model="perks"
                  @change="calcAllPercsBySumm"
                  @blur="formatInputMoney"
                ></LabeledInput>
                <LabeledInput
                  :deniedReg="/[^\d]/g"
                  title="Процент"
                  @change="calcAllSummByPercs"
                  v-model="perksDivPerc"
                ></LabeledInput>
              </div>
              <div class="input_block">
                <LabeledInput
                  pattern="money"
                  title="Выслуга"
                  v-model="vysluga"
                  @change="calcAllPercsBySumm"
                  @blur="formatInputMoney"
                ></LabeledInput>
                <LabeledInput
                  :deniedReg="/[^\d]/g"
                  title="Процент"
                  @change="calcAllSummByPercs"
                  v-model="vyslugaDivPerc"
                ></LabeledInput>
              </div>
              <div class="input_block">
                <LabeledInput
                  pattern="money"
                  title="Секретность"
                  v-model="secrecy"
                  @change="calcAllPercsBySumm"
                  @blur="formatInputMoney"
                ></LabeledInput>
                <LabeledInput
                  :deniedReg="/[^\d]/g"
                  title="Процент"
                  @change="calcAllSummByPercs"
                  v-model="secrecyDivPerc"
                ></LabeledInput>
              </div>
              <div class="input_block">
                <LabeledInput
                  pattern="money"
                  title="Классный чин"
                  v-model="qualification"
                  @change="calcAllPercsBySumm"
                  @blur="formatInputMoney"
                ></LabeledInput>
                <LabeledInput
                  :deniedReg="/[^\d.]/g"
                  title="Процент"
                  @change="calcAllSummByPercs"
                  v-model="qualificationDivPerc"
                ></LabeledInput>
              </div>
              <div class="input_block"></div>
              <div class="input_block"></div>
            </template>
          </LabeledBlock>

          <LabeledBlock lined>
            <template slot="title">
              <h2>МДС</h2>
            </template>
            <template slot="content">
              <LabeledInput disabled title="МДС" v-model="ds"></LabeledInput>
              <LabeledInput disabled title="% от ДС" v-model="dsPerc"></LabeledInput>
              <div class="input_block"></div>
              <div class="input_block"></div>
            </template>
          </LabeledBlock>

          <LabeledBlock lined>
            <template slot="title">
              <h2>Основная пенсия</h2>
            </template>
            <template slot="content">
              <LabeledInput disabled title="Мин. доплата" v-model="minExtraPay"></LabeledInput>
              <LabeledInput
                pattern="money"
                title="Пенсия по старости"
                v-model="gosPension"
                @blur="formatInputMoney"
              ></LabeledInput>
              <LabeledInput
                pattern="money"
                title="Доп. пенсия"
                v-model="extraPension"
                @blur="formatInputMoney"
              ></LabeledInput>
              <div class="input_block"></div>
            </template>
          </LabeledBlock>

          <LabeledBlock lined>
            <template slot="title">
              <h2>Итог</h2>
            </template>
            <template slot="content">
              <LabeledInput disabled title="Госпенсия" v-model="totalPension"></LabeledInput>
              <LabeledInput disabled title="% МДС" v-model="totalPensionAndExtraPay"></LabeledInput>
              <LabeledInput disabled :title="totalExtraPayTitle" v-model="totalExtraPay"></LabeledInput>
              <div class="input_block"></div>
            </template>
          </LabeledBlock>
        </Scrollable>
      </div>

      <div v-show="selectedTab === 'solution'" class="tab-content solution_tab">
        <div class="solution_row button-row">
          <CommonButton
            @click="newSolution.type = 1 , showSolutionPopup = true"
            :text="this.solutions.length ? 'Пересчитать' : 'Определить'"
          ></CommonButton>
          <CommonButton
            v-if="!solutionPaused"
            @click="newSolution.type = 2 , showSolutionPopup = true"
            :text="'Приостановить'"
          ></CommonButton>
          <CommonButton
            v-else
            @click="newSolution.type = 3 , showSolutionPopup = true"
            :text="'Возобновить'"
          ></CommonButton>
          <CommonButton
            @click="newSolution.type = 4 , showSolutionPopup = true"
            :text="'Прекратить'"
          ></CommonButton>
        </div>
        <div class="solution_row">
          <h5>Назначение</h5>
          <h5>Исполнение</h5>
          <h5>Вид решения</h5>
          <h5 class="align-right">Доплата</h5>
          <h5 class="align-right">Пенсия</h5>
          <h5 class="align-right">% МДС</h5>
          <h5 class="flex_80">% ДС</h5>
          <h5 class="align-right">МДС</h5>
          <CommonButton noBackground></CommonButton>
          <CommonButton noBackground></CommonButton>
        </div>
        <Scrollable>
          <div class="solution_row" v-for="solution in solutions" :key="solution.id">
            <h4>{{solution.destination}}</h4>
            <h4>{{solution.execution}}</h4>
            <h4>{{solution.solutionTypeStr}}</h4>
            <h3>
              {{solution.totalExtraPay}}
              <span class="currency">&#8381;</span>
            </h3>
            <h3>
              {{solution.totalPension}}
              <span class="currency">&#8381;</span>
            </h3>
            <h3>
              {{solution.mds}}
              <span class="currency">&#8381;</span>
            </h3>
            <h4 class="flex_80">{{solution.dSperc}}</h4>
            <h3>
              {{solution.ds}}
              <span class="currency">&#8381;</span>
            </h3>
            <BubbleButton noBackground @click="editSolutionComment(solution)"></BubbleButton>
            <CommonButton
              @click="deleteSolution(solution)"
              noBackground
              :disabled="!solution.allowDelete"
              style="opacity: 1"
              html="<i class='fa fa-times remove-button'></i>"
            ></CommonButton>
            <div class="solution-row_comment" v-if="solution.comment">
              <h5>Комментарий:</h5>
              <h4>{{solution.comment}}</h4>
            </div>
          </div>
        </Scrollable>
      </div>

      <div v-show="selectedTab === 'payout'" class="tab-content payout_tab">
        <div class="payout_row">
          <h5 class="flex_160px">Год</h5>
          <h5 class="flex_160px">Месяц</h5>
          <h5 class="flex_160px">Выплачено</h5>
          <h5 class="flex_full">Комментарий</h5>
        </div>
        <Scrollable>
          <div class="payout_row" v-for="payout in payouts" :key="payout.id">
            <h4 class="flex_160px" v-text="payout.year"></h4>
            <h4 class="flex_160px" v-text="payout.month"></h4>
            <h3 class="flex_160px">
              {{payout.amount}}
              <span class="currency">&#8381;</span>
            </h3>
            <LabeledInput
              class="flex_full"
              :class="{ saved: commentSaved === payout.id }"
              placeholder="Комментарий"
              v-model="payout.comment"
              @blur="editComment(payout)"
            ></LabeledInput>
          </div>
        </Scrollable>
        <div class="bank_card-container">
          <div class="bank_card" v-if="bankCard.type === 0">
            <h5>Номер карты Сбербанка</h5>
            <h4>{{bankCard.number || 'не указан'}}</h4>
            <h5>Окончание действия</h5>
            <h4>
              {{(bankCard.validThru) || 'не указано'}}
              <span
                class="valid_thru error"
                v-if="bankCard.validThru && bankCard.validRemains <= 0"
              >(Карта просрочена)</span>
              <span
                class="valid_thru warning"
                v-if="bankCard.validThru && 0 < bankCard.validRemains && bankCard.validRemains <= 30"
              >(Осталось {{ageToString(0, 0, bankCard.validRemains)}})</span>
            </h4>
          </div>
          <div class="bank_card" v-if="bankCard.type === 1 || bankCard.type === undefined">
            <h5>Номер счета ЕМБ-банка</h5>
            <h4>{{bankCard.account || 'не указан'}}</h4>
          </div>
          <EditButton @click="showBankCardPopup = true"></EditButton>
        </div>
      </div>
    </div>
    <div class="command_menu">
      <CrossButton @click="cancel"></CrossButton>
      <SaveButton v-show="selectedTab === 'main_info'" @click="saveMainInfo"></SaveButton>
      <SaveButton
        v-show="selectedTab === 'extra_pay' && extraPayChanged"
        @click="newSolution.type = 1, showSolutionPopup = true"
      ></SaveButton>
      <PlusButton v-show="selectedTab === 'work_info'" @click="addWorkInfo"></PlusButton>
      <SymbolCButton v-show="selectedTab === 'main_info'" @click="sendSnilsRequest"></SymbolCButton>
    </div>
    <WorkInfoPopup
      :shown="showWorkPopup"
      :title="workPopupTitle"
      :editWork="editWork"
      @close="showWorkPopup = false"
      @click="sendWorkInfo"
    ></WorkInfoPopup>
    <Popup
      :shown="showWorkApprovePopup"
      title="Подтверждение стажа"
      @close="showWorkApprovePopup = false"
    >
      <h5 class="width-240">Подача документов</h5>
      <LabeledInput pattern="date" placeholder="дд.мм.гггг" v-model="docsSubmitDate"></LabeledInput>
      <div class="popup_br"></div>
      <h5 class="width-240">Назначение</h5>
      <LabeledInput pattern="date" placeholder="дд.мм.гггг" v-model="docsDestinationDate"></LabeledInput>
      <div class="popup_br"></div>
      <h4
        class="stage-approvement"
        :class="{ selected: stageApproved }"
        v-show="workInfos && workInfos.length"
        @click="stageApproved = !stageApproved"
      >
        <i class="fa fa-check"></i>Стаж подтвержден
      </h4>
      <div class="popup_br"></div>
      <CommonButton @click="approveWorkInfo" :text="'Сохранить'"></CommonButton>
    </Popup>
    <Popup
      :shown="showSolutionPopup"
      :title="solutionPopupTitle"
      @close="showSolutionPopup = false"
    >
      <CheckBox
        v-show="this.selectedTab === 'extra_pay'"
        class="width-180"
        :title="'Создать решение'"
        v-model="newSolution.createSolution"
      ></CheckBox>
      <div class="popup_br"></div>
      <h5 class="width-140 margin-left-50">Назначение</h5>
      <LabeledInput
        class="width-240"
        pattern="date"
        placeholder="дд.мм.гггг"
        v-model="newSolution.destination"
        :disabled="!newSolution.createSolution"
      ></LabeledInput>
      <div class="popup_br"></div>
      <h5 class="width-140 margin-left-50">Исполнение</h5>
      <LabeledInput
        class="width-240"
        pattern="date"
        placeholder="дд.мм.гггг"
        v-model="newSolution.execution"
        :disabled="!newSolution.createSolution"
      ></LabeledInput>
      <div class="popup_br"></div>
      <h5 class="width-140 margin-left-50">Комментарий</h5>
      <LabeledInput
        class="width-240"
        v-model="newSolution.comment"
        placeholder="комментарий"
        :disabled="!newSolution.createSolution"
      ></LabeledInput>
      <div class="popup_br"></div>
      <CommonButton
        @click="sendSolution"
        :disabled="!newSolution.destination || !newSolution.execution"
        :text="this.selectedTab === 'extra_pay' ? 'Сохранить' : 'Отправить'"
      ></CommonButton>
    </Popup>
    <Popup
      :shown="showBankCardPopup"
      :title="bankCardPopupTitle"
      @close="showBankCardPopup = false"
    >
      <h5 class="width-200">Номер карты Сбербанка</h5>
      <LabeledInput
        :disabled="newBankCardType === 1"
        class="width-240"
        :deniedReg="/[^\d\- ]/g"
        :maxlength="24"
        v-model="newBankCard.number"
        placeholder="номер"
      ></LabeledInput>
      <div class="popup_br"></div>
      <h5 class="width-200">Окончание действия</h5>
      <LabeledInput
        :disabled="newBankCardType === 1"
        class="width-240"
        pattern="date"
        placeholder="дд.мм.гггг"
        v-model="newBankCard.validThru"
      ></LabeledInput>
      <div class="popup_br"></div>
      <h5 class="width-200">Номер счета ЕМБ-банка</h5>
      <LabeledInput
        :disabled="newBankCardType === 0"
        class="width-240"
        :deniedReg="/[^\d\- ]/g"
        :maxlength="20"
        v-model="newBankCard.account"
        placeholder="номер"
      ></LabeledInput>
      <div class="popup_br"></div>
      <CommonButton
        @click="editBankCard"
        :disabled="!(newBankCard.number || newBankCard.validThru) && !(newBankCard.account)"
        :text="'Отправить'"
      ></CommonButton>
    </Popup>
    <Popup
      :title="'Редактировать комментарий к решению'"
      :shown="showEditSolutionCommentPopup"
      @close="showEditSolutionCommentPopup = false"
    >
      <LabeledInput v-model="newSolutionComment" placeholder="комментарий"></LabeledInput>
      <div class="popup_br"></div>
      <CommonButton @click="acceptSolutionComment" :text="'Сохранить'"></CommonButton>
    </Popup>
    <Popup
      class="confirm_delete_solution_popup"
      :title="''"
      :shown="showConfirmDeleteSolutionPopup"
      @close="showConfirmDeleteSolutionPopup = false"
    >
      <h2>Вы действительно хотите удалить решение? Эту операцию нельзя будет отменить.</h2>
      <CommonButton @click="confirmDeleteSolution" :text="'Удалить'"></CommonButton>
      <CommonButton @click="showConfirmDeleteSolutionPopup = false" :text="'Отмена'"></CommonButton>
    </Popup>

    <RequestResultPopup @close="requestResult.showPopup = false" :requestResult="requestResult"></RequestResultPopup>
  </div>
</template>

<style>
.edit_layout {
  height: calc(100% - 40px);
}
.card-edit {
  flex: 0 0;
  flex-basis: calc(100% - 112px);
  height: 100%;
  border-radius: 4px;
  background-color: #3f3176;
  border: 1px solid #493c80;
  box-sizing: border-box;
  margin: 0 0 0 16px;
}
.card-edit .tab-navigation {
  border-color: #5d4ca3;
  display: flex;
  justify-content: space-between;
  flex-wrap: nowrap;
  width: 100%;
}
.card-edit .tab-navigation .common-button {
  border-radius: 0;
  flex: 1 1 auto;
  color: #c6bcee;
  padding: 23px 0;
}
.card-edit .tab-navigation .common-button.selected,
.card-edit .tab-navigation .common-button:hover,
.card-edit .tab-navigation .common-button:active {
  color: #ffffff;
}
.card-edit .tab-navigation .common-button.selected {
  background-color: #3f3176;
  border: 1px solid #3f3176;
}
.tab-navigation .common-button:first-child {
  border-top-left-radius: 4px;
}
.tab-navigation .common-button:last-child {
  border-top-right-radius: 4px;
}

.card-edit .tab-content {
  padding: 12px 48px 24px;
  box-sizing: border-box;
  display: flex;
  flex-direction: row;
  justify-content: space-between;
  align-content: flex-start;
  align-items: baseline;
  flex-wrap: wrap;
  height: calc(100% - 64px);
}
.card-edit .tab-content.payout_tab .scrollable-container,
.card-edit .tab-content.work-info_tab .scrollable-container,
.card-edit .tab-content.solution_tab .scrollable-container {
  width: calc(100% + 36px);
  margin-right: -36px;
  flex: 0 0 auto;
}
.card-edit .tab-content.work-info_tab .scrollable-container {
  height: calc(100% - 272px);
}
.card-edit .tab-content.payout_tab .scrollable-container,
.card-edit .tab-content.solution_tab .scrollable-container {
  height: calc(100% - 190px);
}
.card-edit .tab-content.extra_pay_tab .scrollable-container,
.card-edit .tab-content.main-info_tab .scrollable-container {
  height: 100%;
  width: 100%;
  margin-bottom: 48px;
}
.card-edit .tab-content.extra_pay_tab .input_block,
.card-edit .tab-content.extra_pay_tab .dropdown,
.card-edit .tab-content.extra_pay_tab .labeled-input {
  margin: 24px 0 8px;
  flex: 0 0;
  flex-basis: calc((100% - 72px) / 4);
}
.card-edit .tab-content.extra_pay_tab .labeled_block-content,
.card-edit .tab-content.main-info_tab .scrollable-content {
  display: inline-flex;
  flex-direction: row;
  justify-content: space-between;
  justify-items: flex-start;
  align-content: flex-end;
  align-items: flex-end;
  flex-wrap: wrap;
}
.card-edit .tab-content.extra_pay_tab .labeled_block-content {
  width: 100%;
  padding: 0 0 12px;
}
.card-edit .tab-content.extra_pay_tab .labeled_block h2 {
  margin: 12px 24px 12px 0;
}
.card-edit .tab-content h1 {
  margin: 24px 0 0;
  flex: 0 0 100%;
}

.card-edit .tab-content .input_block {
  display: flex;
  flex-wrap: nowrap;
  justify-content: space-between;
  align-items: flex-end;
}
.card-edit .tab-content .input_block,
.card-edit .tab-content .dropdown,
.card-edit .tab-content .labeled-input {
  margin: 24px 0 8px;
  flex: 0 0;
  flex-basis: calc((100% - 64px) / 5);
}
.card-edit .tab-content .labeled-input .text-input {
  background-color: #503f96;
}
.card-edit .tab-content .labeled-input .text-input[disabled] {
  background-color: #2c205e;
}
.card-edit .tab-content .dropdown.double,
.card-edit .tab-content .prompt_input_container.double,
.card-edit .tab-content .labeled-input.double {
  flex: 0 0;
  flex-basis: calc((100% - 64px) / 5 * 2 + 16px);
}
.card-edit .tab-content .input_block .labeled-input {
  margin: 0;
  flex: 0 0;
  flex-basis: calc(100% - 124px);
}
.card-edit .tab-content .input_block .labeled-input:last-child {
  flex-basis: 100px;
}
.button-container {
  margin-top: 24px;
  flex: 0 0 100%;
  display: flex;
  justify-content: space-around;
}
.button-container .common-button {
  flex: 0 0 240px;
}

.solution_row,
.payout_row,
.card-edit .work-info_row {
  flex: 0 0 100%;
  display: flex;
  flex-wrap: wrap;
  justify-content: space-between;
  align-content: center;
  align-items: center;
  border-bottom: 1px solid #534590;
  padding: 12px 0;
  margin: 0;
}
.card-edit .tab-content .payout_row * {
  margin: 0;
}
.card-edit .work-info_row h4,
.card-edit .work-info_row h5 {
  flex: 0 0;
  flex-basis: calc((100% - 226px) / 4);
  margin-left: 12px;
  margin-right: 12px;
}
.payout_row .flex_160px {
  flex: 0 0 160px;
}
.payout_row .flex_full {
  flex: 1 1 auto !important;
  word-break: break-word;
}
.solution_row h5,
.solution_row h4,
.solution_row h3 {
  flex: 0 0;
  flex-basis: calc((100% - 208px) / 7);
}
.solution_row h3 {
  text-align: right;
  padding-right: 8px;
  box-sizing: border-box;
  white-space: nowrap;
}
.currency {
  color: black;
}
.solution_row.button-row {
  justify-content: flex-start;
}
.solution_row.button-row .common-button {
  width: 240px;
  margin-right: 24px;
}
.solution_row .flex_80 {
  flex: 0 0 80px;
  text-align: center;
}
.solution_row .align-right {
  text-align: right;
  padding-right: 24px;
}
.solution_row .solution-row_comment {
  flex: 0 0;
  flex-basis: calc(100% - 64px);
  display: flex;
  justify-content: space-between;
}
.solution_row .solution-row_comment h5 {
  flex: 1 1 20%;
}
.solution_row .solution-row_comment h4 {
  flex: 1 1 80%;
}

.card-edit .bank_card-container {
  padding-bottom: 32px;
  width: calc(100% - 512px);
  display: flex;
  flex-wrap: wrap;
  position: absolute;
  bottom: 0;
}
.card-edit .bank_card-container .bank_card {
  flex-basis: calc(100% - 112px);
  display: flex;
  flex-wrap: wrap;
  align-items: center;
}
.card-edit .bank_card-container h2 {
  margin-left: 0;
  flex: 0 0 100%;
}
.card-edit .bank_card-container h5 {
  flex: 0 0 30%;
}
.card-edit .bank_card-container h4 {
  flex: 0 0 70%;
}
.card-edit .bank_card-container .common-button {
  margin: 24px;
}
.valid_thru.error {
  color: red;
}
.valid_thru.warning {
  color: #f1b238;
}

.edit_layout .popup-content > .labeled-input {
  flex: 0 0 45%;
}
.edit_layout .popup-content .dropdown {
  flex: 0 0 45%;
}
.edit_layout .popup-content .prompt_input_container {
  flex: 0 0 45%;
}
.edit_layout .popup-content > .common-button {
  flex: 0 0 50%;
}
.edit_layout .confirm_delete_solution_popup .popup-content .common-button {
  flex: 0 0 240px;
  margin: 0 24px;
}

.edit_layout .work-info_total-row h4,
.edit_layout .work-info_total-row h5 {
  margin: 8px 0;
}
.edit_layout .work-info_total-row > h2,
.edit_layout .work-info_total-row > h3 {
  margin-top: 20px;
}
.stage_block {
  position: absolute;
  bottom: 48px;
  right: 144px;
}
.stage-approvement {
  display: inline-block;
  width: 240px;
}
.popup-content .stage-approvement {
  margin: 16px 0 0;
  flex: 0 0 100%;
  text-align: center;
}
.stage-approvement .fa {
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
.stage-approvement:hover .fa {
  background-color: #8a7ec0;
}
.stage-approvement:active .fa {
  background-color: #6253a2;
  color: #ffffff;
}
.stage-approvement.selected .fa {
  color: #ffffff;
}

.text-input.saved,
.labeled-input.saved {
  border-color: #00ca00;
}

@media (max-width: 1440px) {
  .tab-content {
    padding: 16px 32px;
  }
  .tab-content .dropdown,
  .tab-content .labeled-input {
    flex: 0 0 30%;
  }
  .tab-content .labeled-input.double {
    flex: 0 0 65%;
  }
}
</style>

<script lang="ts">
import Vue from 'vue';
import CommonButton from '@/components/CommonButton.vue';
import BubbleButton from '@/components/Common/Buttons/BubbleButton.vue';
import CrossButton from '@/components/Common/Buttons/CrossButton.vue';
import SaveButton from '@/components/Common/Buttons/SaveButton.vue';
import EditButton from '@/components/Common/Buttons/EditButton.vue';
import PlusButton from '@/components/Common/Buttons/PlusButton.vue';
import SymbolCButton from '@/components/Common/Buttons/SymbolCButton.vue';
import CheckBox from '@/components/Common/CheckBox.vue';
import LabeledInput from '@/components/LabeledInput.vue';
import LabeledInputWithPrompt from '@/components/LabeledInputWithPrompt.vue';
import Scrollable from '@/components/Scrollable.vue';
import LabeledBlock from '@/components/Common/LabeledBlock.vue';
import DropDown from '@/components/DropDown.vue';
import Popup from '@/components/Popup.vue';
import WorkInfoPopup from '@/components/CardEdit/Popups/WorkInfoPopup.vue';
import RequestResultPopup from '@/components/Common/Popups/RequestResultPopup.vue';
import axios, { AxiosError, AxiosResponse } from 'axios';
import { formatDate, ageToString, formatMoney } from '@/models/utils/stringBuilder';
import {
  calculatePercents,
  calculateSumm,
  multiplyStrings,
  summStrings,
  substractStrings,
  numericDiffStrings,
  floatParse,
} from '@/models/utils/calculations';
import CardEditData from '@/models/cardEditData';
import PersonInfo from '@/models/person/personInfo';
import ExtraPay from '@/models/person/extraPayInfo';
import WorkInfo from '@/models/person/workInfo';
import PersonBankCard from '@/models/person/personBankCard';
import RequestSender from '@/requestSender';
export default Vue.extend({
  data() {
    return {
      needRecountStageMessage: '. Для добавления нового решения перейдите на вкладку "Решения"',
      card: new PersonInfo(),
      selectedTab: '',
      showWorkPopup: false,
      showWorkApprovePopup: false,
      workPopupTitle: '',
      extraPayChanged: false,
      showSolutionPopup: false,
      showBankCardPopup: false,
      editWork: new WorkInfo({}),
      stageApproved: false,
      docsSubmitDate: formatDate(new Date().toISOString()),
      docsDestinationDate: formatDate(new Date().toISOString()),
      commentSaved: '',
      showEditSolutionCommentPopup: false,
      solutionForEdit: { id: '', comment: '' },
      solutionForDeleteId: '',
      showConfirmDeleteSolutionPopup: false,
      newSolutionComment: '',
      newSolution: {
        type: -1,
        createSolution: true,
        destination: formatDate(new Date().toISOString()),
        execution: formatDate(new Date().toISOString()),
        comment: '',
      },
      newBankCard: new PersonBankCard({}),
      requestResult: {
        success: true,
        message: '',
        showPopup: false,
      },
      //#region extraPay
      matSupportMultiplier: '1',
      dsPerc: '0.00',
      extraPension: '0.00',
      gosPension: '0.00',
      perks: '0.00',
      perksDivPerc: '0.00',
      premium: '0.00',
      premiumPerc: '0.00',
      qualification: '0.00',
      qualificationDivPerc: '0.00',
      salary: '0.00',
      secrecy: '0.00',
      secrecyDivPerc: '0.00',
      uralMultiplier: '0.00',
      variantId: ' ',
      ignoreGosPension: false,
      variantNumber: '0.00',
      vysluga: '0.00',
      vyslugaDivPerc: '0.00',
      //#endregion extraPay
      extraPayWatchers: [
        'salary',
        'uralMultiplier',
        'variantId',
        'variantNumber',
        'extraPension',
        'gosPension',
        'perks',
        'perksDivPerc',
        'premium',
        'premiumPerc',
        'qualification',
        'qualificationDivPerc',
        'vysluga',
        'vyslugaDivPerc',
        'secrecy',
        'secrecyDivPerc',
      ],
    };
  },
  props: {
    cardId: String,
    tab: String,
  },
  mounted() {
    if (this.cardId) {
      this.$store.commit('setSelectedCardId', this.cardId);
    }

    this.selectedTab = 'main_info';
    if (this.tab) {
      this.selectedTab = this.tab;
    }
    if (this.selectedTab === 'extra_pay') {
      setTimeout(() => this.fillExtraPay(), 0);
    }
    this.requestSender.loadCardEditData();
  },
  computed: {
    //#region extraPay
    ds(): string {
      return formatMoney(summStrings([
        this.salaryMultiplied,
        this.premium,
        this.materialSupport,
        this.perks,
        this.vysluga,
        this.secrecy,
        this.qualification,
      ]));
    },
    salaryMultiplied(): string {
      return formatMoney(multiplyStrings([this.extraPay.salary, this.extraPay.uralMultiplier]));
    },
    totalPension(): string {
      return formatMoney(summStrings([
        this.gosPension,
        this.extraPension,
      ]));
    },
    totalPensionAndExtraPay(): string {
      return formatMoney(calculateSumm(this.ds, this.dsPerc));
    },
    totalExtraPay(): string {
      const right = this.ignoreGosPension ? '0' : this.totalPension;
      return formatMoney(substractStrings(this.totalPensionAndExtraPay, right, this.minExtraPay));
    },
    materialSupport(): string {
      return formatMoney(this.extraPay.materialSupport);
      // return formatMoney(multiplyStrings([this.salary, this.matSupportMultiplier]));
    },
    //#endregion extraPay
    requestSender(): RequestSender {
      return this.$store.getters.requestSender;
    },
    selectedCardId(): string {
      return this.$store.getters.selectedCardId;
    },
    selectedCard(): PersonInfo {
      return this.$store.getters.selectedCard;
    },
    workEndDate(): string {
      return (this.selectedCard && this.selectedCard.works)
        ? (this.selectedCard.works[0] || {}).endDate
        : '';
    },
    cardEditData(): CardEditData {
      return this.$store.getters.cardEditData;
    },
    extraPayVariants(): Array<{}> {
      return this.$store.getters.extraPayVariants
        .map((el: any) => {
          return {
            value: el.id,
            title: el.number,
          };
        });
    },
    workInfos(): WorkInfo[] {
      return this.$store.getters.selectedCard.works;
    },
    extraPay(): ExtraPay {
      return this.$store.getters.selectedCard.extraPay;
    },
    minExtraPay(): string {
      return this.$store.getters.minExtraPay;
    },
    totalExtraPayTitle(): string {
      return this.card.payoutTypeId === '0235a5e2-fe16-4967-bb8a-dd91e9bb76bd'
        ? 'Пенсия по выслуге лет'
        : 'Доплата';
    },
    solutions(): Array<{}> {
      return this.$store.getters.selectedCard.solutions;
    },
    payouts(): Array<{}> {
      return this.$store.getters.selectedCard.payouts.sort((a: any, b: any) => {
        return b.compare(a);
      });
    },
    solutionPaused(): boolean {
      return this.$store.getters.selectedCard.solutions.filter((el: any) =>
        el.solutionTypeStr.indexOf('Приостановить') >= 0,
      ).length >
        this.$store.getters.selectedCard.solutions.filter((el: any) =>
          el.solutionTypeStr.indexOf('Возобновить') >= 0,
        ).length;
    },
    solutionPopupTitle(): string {
      switch (this.newSolution.type) {
        case 1: return this.solutions.length
          ? 'Пересчитать'
          : 'Определить';
        case 2: return 'Приостановить';
        case 3: return 'Возобновить';
        case 4: return 'Прекратить';
      }
      return '';
    },
    bankCard(): any {
      return this.$store.getters.selectedCard
        ? this.$store.getters.selectedCard.bankCard || {}
        : {};
    },
    bankCardPopupTitle(): string {
      return this.bankCard.id ? 'Добавить реквизиты карты' : 'Редактировать реквизиты карты';
    },
    newBankCardType(): number | undefined {
      return this.newBankCard.number
        ? 0
        : this.newBankCard.account
          ? 1
          : undefined;
    },
  },
  watch: {
    //#region extraPay
    ds() {
      this.deactivateWatcher('extraPay');
      this.extraPay.ds = this.ds;
      this.activateWatcherAsync('extraPay');
    },
    dsPerc() {
      // this.deactivateWatcher('extraPay');
      this.extraPay.dsPerc = this.dsPerc;
      // this.activateWatcherAsync('extraPay');
    },
    salary() {
      this.deactivateWatcher('extraPay');
      this.extraPayChanged = true;
      this.extraPay.salary = formatMoney(this.salary);
      this.extraPay.materialSupport = formatMoney(multiplyStrings([this.salary, this.matSupportMultiplier]));
      this.activateWatcherAsync('extraPay');
    },
    uralMultiplier() {
      this.deactivateWatcher('extraPay');
      this.extraPayChanged = true;
      this.extraPay.uralMultiplier = this.uralMultiplier;
      this.activateWatcherAsync('extraPay');
    },
    salaryMultiplied() {
      this.extraPay.salaryMultiplied = this.salaryMultiplied;
      /* При смене зп*к-т пересчитаваем суммы */
      this.deactivateWatcher('perks');
      this.deactivateWatcher('premium');
      this.deactivateWatcher('qualification');
      this.deactivateWatcher('secrecy');
      this.deactivateWatcher('vysluga');
      this.perks = formatMoney(calculateSumm(this.salaryMultiplied, this.perksDivPerc));
      this.premium = formatMoney(calculateSumm(this.salaryMultiplied, this.premiumPerc));
      this.qualification = formatMoney(calculateSumm(this.salaryMultiplied, this.qualificationDivPerc));
      this.secrecy = formatMoney(calculateSumm(this.salaryMultiplied, this.secrecyDivPerc));
      this.vysluga = formatMoney(calculateSumm(this.salaryMultiplied, this.vyslugaDivPerc));
      this.activateWatcherAsync('perks');
      this.activateWatcherAsync('premium');
      this.activateWatcherAsync('qualification');
      this.activateWatcherAsync('secrecy');
      this.activateWatcherAsync('vysluga');
      /* */
    },
    perks() {
      this.deactivateWatcher('extraPay');
      this.extraPayChanged = true;
      this.extraPay.perks = formatMoney(this.perks);
      this.activateWatcherAsync('extraPay');
    },
    perksDivPerc(val: string, old: string) {
      this.deactivateWatcher('extraPay');
      this.deactivateWatcher('perks');
      this.extraPayChanged = true;
      this.extraPay.perksDivPerc = this.perksDivPerc;
      this.activateWatcherAsync('extraPay');
    },
    premium() {
      this.deactivateWatcher('extraPay');
      this.extraPayChanged = true;
      this.extraPay.premium = formatMoney(this.premium);
      this.activateWatcherAsync('extraPay');
    },
    premiumPerc(val: string, old: string) {
      this.deactivateWatcher('extraPay');
      this.extraPayChanged = true;
      this.extraPay.premiumPerc = this.premiumPerc;
      this.activateWatcherAsync('extraPay');
    },
    qualification() {
      this.deactivateWatcher('extraPay');
      this.extraPayChanged = true;
      this.extraPay.qualification = formatMoney(this.qualification);
      this.activateWatcherAsync('extraPay');
    },
    qualificationDivPerc(val: string, old: string) {
      this.deactivateWatcher('extraPay');
      this.qualificationDivPerc = parseFloat(val).toString();
      this.extraPayChanged = true;
      this.extraPay.qualificationDivPerc = this.qualificationDivPerc;
      this.activateWatcherAsync('extraPay');
    },
    secrecy() {
      this.deactivateWatcher('extraPay');
      this.extraPayChanged = true;
      this.extraPay.secrecy = formatMoney(this.secrecy);
      this.activateWatcherAsync('extraPay');
    },
    secrecyDivPerc(val: string, old: string) {
      this.deactivateWatcher('extraPay');
      this.extraPayChanged = true;
      this.extraPay.secrecyDivPerc = this.secrecyDivPerc;
      this.activateWatcherAsync('extraPay');
    },
    vysluga() {
      this.deactivateWatcher('extraPay');
      this.extraPayChanged = true;
      this.extraPay.vysluga = formatMoney(this.vysluga);
      this.activateWatcherAsync('extraPay');
    },
    vyslugaDivPerc(val: string, old: string) {
      this.deactivateWatcher('extraPay');
      this.extraPayChanged = true;
      this.extraPay.vyslugaDivPerc = this.vyslugaDivPerc;
      this.activateWatcherAsync('extraPay');
    },
    materialSupport() {
      // this.extraPay.materialSupport = this.materialSupport;
    },
    matSupportMultiplier() {
      this.deactivateWatcher('extraPay');
      this.extraPay.materialSupport = formatMoney(multiplyStrings([this.salary, this.matSupportMultiplier]));
      this.activateWatcherAsync('extraPay');
    },
    variantId() {
      this.deactivateWatcher('extraPay');
      const variant = (this.$store.getters.extraPayVariants || [])
        .find((el: any) => el.id === this.variantId);
      if (!variant) {
        return;
      }

      this.extraPay.variantId = this.variantId;
      if (variant.id === '00000000-0000-0000-0000-000000000000') {
        this.ignoreGosPension = false;
        return;
      }

      this.ignoreGosPension = variant.ignoreGosPension;
      if (variant.uralMultiplier || variant.uralMultiplier === 0) {
        this.uralMultiplier = variant.uralMultiplier.toString();
      }
      if (variant.premiumPerc || variant.premiumPerc === 0) {
        this.premiumPerc = variant.premiumPerc.toString();
      }
      if (variant.vyslugaDivPerc || variant.vyslugaDivPerc === 0) {
        this.vyslugaDivPerc = variant.vyslugaDivPerc.toString();
      }
      if (variant.matSupportMultiplier || variant.matSupportMultiplier === 0) {
        this.matSupportMultiplier = variant.matSupportMultiplier.toString();
      } else {
        this.matSupportMultiplier = this.extraPay.matSupportMultiplier;
      }
      this.activateWatcherAsync('extraPay');
    },
    gosPension() {
      this.extraPayChanged = true;
      this.extraPay.gosPension = this.gosPension;
    },
    extraPension() {
      this.extraPayChanged = true;
      this.extraPay.extraPension = this.extraPension;
    },
    totalPension() {
      this.extraPay.totalPension = this.totalPension;
    },
    totalPensionAndExtraPay() {
      this.extraPay.totalPensionAndExtraPay = this.totalPensionAndExtraPay;
    },
    totalExtraPay() {
      this.extraPay.totalExtraPay = this.totalExtraPay;
    },
    //#endregion
    selectedTab(val): void {
      if (this.selectedCardId) {
        this.card.fillFrom(this.$store.getters.selectedCard);
      }
      if (!val) {
        return;
      }
      switch (val) {
        case 'main_info':
          this.requestSender.getAllExtraPayVariants();
          if (this.selectedCardId) {
            this.requestSender.getAllExtraPayVariants();
            this.requestSender.loadCard(this.selectedCardId).then(() => {
              this.requestSender.loadExtraPay(this.selectedCardId);
              this.card.fillFrom(this.$store.getters.selectedCard);
              this.fillCardIds();
            });
          }
          break;
        case 'work_info':
          this.requestSender.loadCard(this.selectedCardId);
          this.requestSender.getAllFunctions();
          this.requestSender.getAllOrganizations();
          /* TODO - убрать, когда поправят баг с пересчетом в стаже */
          this.requestSender.loadSolutions(this.selectedCardId);
          break;
        case 'extra_pay':
          this.requestSender.getMinExtraPay();
          this.requestSender.getAllExtraPayVariants();
          this.requestSender.loadExtraPay(this.selectedCardId)
            .then(() => this.fillExtraPay());
          break;
        case 'solution':
          this.requestSender.loadSolutions(this.selectedCardId);
          break;
        case 'payout':
          this.requestSender.getPersonBankCard(this.selectedCardId);
          break;
      }
      this.$router.push('/edit?cardId=' + (this.selectedCardId || '') + '&tab=' + val);
    },
    selectedCardId(val: string): void {
      if (val) {
        this.requestSender.loadCard(val)
          .then(() => {
            this.card.fillFrom(this.$store.getters.selectedCard);
            this.requestSender.getPersonBankCard(this.selectedCardId);
            this.requestSender.loadSolutions(this.selectedCardId);
            this.fillExtraPay();
            this.fillCardIds();
          });
      } else {
        this.fillCardIds();
      }
    },
    cardEditData: {
      handler(): void {
        if (!this.cardId) {
          this.card.number = this.cardEditData.nextNumber.toString();
        }
        this.fillCardIds();
      },
      deep: true,
    },
    bankCard: {
      handler() {
        this.newBankCard.id = this.bankCard.id;
        this.newBankCard.personRootId = this.bankCard.personRootId || this.selectedCardId;
        this.newBankCard.type = this.bankCard.type;
        this.newBankCard.number = this.bankCard.number;
        this.newBankCard.validThru = this.bankCard.validThru;
        this.newBankCard.account = this.bankCard.account;
        this.newBankCard.validRemains = this.bankCard.validRemains;
      },
      deep: true,
    },
    'requestResult.showPopup'(val: boolean) {
      if (!val && this.requestResult.success) {
        switch (this.selectedTab) {
          case 'main_info':
            this.$router.push('/edit?cardId=' + (this.selectedCardId || '') + '&tab=work_info');
            this.selectedTab = 'work_info';
            break;
          case 'extra_pay':
            this.$router.push('/edit?cardId=' + (this.selectedCardId || '') + '&tab=solution');
            this.selectedTab = 'solution';
            break;
        }
      }
    },
    'selectedCard.worksApproved'() {
      this.card.worksApproved = this.selectedCard.worksApproved;
      this.stageApproved = this.selectedCard.worksApproved;
    },
    'selectedCard.docsSubmitDate'() {
      this.docsSubmitDate = this.selectedCard.docsSubmitDate;
    },
    'selectedCard.docsDestinationDate'() {
      this.docsDestinationDate = this.selectedCard.docsDestinationDate;
    },
    showWorkApprovePopup(val) {
      if (val) {
        this.stageApproved = this.card.worksApproved;
      }
    },
    showConfirmDeleteSolutionPopup(val) {
      if (!val) {
        this.solutionForDeleteId = '';
      }
    },
    showSolutionPopup(val) {
      if (val) {
        this.newSolution.createSolution = true;
      }
    },
  },
  methods: {
    getMonthNumber(month: string): number {
      switch (month.toLowerCase()) {
        case 'январь': return 1;
        case 'февраль': return 2;
        case 'март': return 3;
        case 'апрель': return 4;
        case 'май': return 5;
        case 'июнь': return 6;
        case 'июль': return 7;
        case 'август': return 8;
        case 'сентябрь': return 9;
        case 'октябрь': return 10;
        case 'ноябрь': return 11;
        case 'декабрь': return 12;
      }
      return 0;
    },
    fillCardIds() {
      this.card.additionalPensionId =
        this.cardEditData.getValueByTitle('additionalPensionTypes', this.card.additionalPension);
      this.card.districtId = this.cardEditData.getValueByTitle('districts', this.card.district);
      this.card.employeeTypeId = this.cardEditData.getValueByTitle('employmentTypes', this.card.employeeType);
      this.card.pensionTypeId = this.cardEditData.getValueByTitle('pensionTypes', this.card.pensionType);
      this.card.payoutTypeId = this.cardEditData.getValueByTitle('payoutTypes', this.card.payoutType);
      this.card.documentTypeId = this.cardEditData.getValueByTitle('personDocumentTypes', this.card.documentType);
    },
    fillExtraPay() {
      this.deactivateExtraPayWatchers();

      this.salary = this.extraPay.salary;
      this.uralMultiplier = this.extraPay.uralMultiplier;
      this.variantId = this.extraPay.variantId;
      this.dsPerc = this.extraPay.dsPerc;
      this.variantNumber = this.extraPay.variantNumber;
      this.extraPension = this.extraPay.extraPension;
      this.gosPension = this.extraPay.gosPension;
      this.perks = this.extraPay.perks;
      this.perksDivPerc = formatMoney(calculatePercents(this.salaryMultiplied, this.perks), 0);
      this.premium = this.extraPay.premium;
      this.premiumPerc = formatMoney(calculatePercents(this.salaryMultiplied, this.premium), 0);
      this.qualification = this.extraPay.qualification;
      this.qualificationDivPerc = formatMoney(calculatePercents(this.salaryMultiplied, this.qualification), 0);
      this.vysluga = this.extraPay.vysluga;
      this.vyslugaDivPerc = formatMoney(calculatePercents(this.salaryMultiplied, this.vysluga), 0);
      this.secrecy = this.extraPay.secrecy;
      this.secrecyDivPerc = formatMoney(calculatePercents(this.salaryMultiplied, this.secrecy), 0);
      if (!this.extraPay.variantId || this.extraPay.variantId === '00000000-0000-0000-0000-000000000000') {
        this.matSupportMultiplier = this.extraPay.matSupportMultiplier;
      } else {
        const variant = (this.$store.getters.extraPayVariants || [])
          .find((el: any) => el.id === this.variantId);
        if (!variant || !variant.matSupportMultiplier) {
          this.matSupportMultiplier = this.extraPay.matSupportMultiplier;
        }
      }

      this.activateExtraPayWatchersAsync();
    },
    ageToString(y: number, m: number, d: number) {
      return ageToString(y, m, d);
    },
    parseError(error: AxiosError, type: string): string {
      if (!error.response || error.response.status >= 500 || error.response.status < 400) {
        return 'Ошибка отправки запроса, попробуйте повторить попытку позднее';
      }

      if (typeof error.response.data === 'string') {
        return error.response.data;
      }
      if (typeof error.response.data === 'object') {
        let fieldLocale;
        switch (type) {
          case 'main_info':
            fieldLocale = (PersonInfo.fieldTitleLocale as any);
            break;
          case 'extra_pay':
            fieldLocale = (ExtraPay.fieldTitleLocale as any);
            break;
          default: fieldLocale = {};
        }
        let result = 'Некорректные данные: ';
        for (const i in error.response.data) {
          if (error.response.data.hasOwnProperty(i)) {
            result += (fieldLocale as any)[i.toLowerCase()];
            const message = error.response.data[i].join(', ').replace('The input was not valid.', '').trim();
            if (message) {
              result += ' - ' + error.response.data[i].toString().replace('The input was not valid.', '');
            }
            result += ', ';
          }
        }
        return result;
      }
      return 'Ошибка отправки запроса, попробуйте повторить попытку позднее';
    },
    saveMainInfo() {
      this.requestSender.createEditCard(this.card)
        .then((resp: AxiosResponse) => {
          this.requestResult.success = true;
          this.requestResult.message = 'Данные карты успешно сохранены';
          this.requestResult.showPopup = true;
        }).catch((resp: AxiosError) => {
          this.requestResult.success = false;
          this.requestResult.message = this.parseError(resp, 'main_info');
          this.requestResult.showPopup = true;
        });
    },
    saveExtraPay() {
      const data = this.extraPay.toSendModel(this.card.id) as any;
      data.createSolution = this.newSolution.createSolution;
      data.destinationDate = this.newSolution.destination;
      data.executionDate = this.newSolution.execution;
      data.comment = this.newSolution.comment;
      return this.requestSender.saveExtraPay(data)
        .then((resp: AxiosResponse) => {
          this.requestResult.success = true;
          this.requestResult.message = 'Данные расчета доплат успешно сохранены';
          this.requestResult.showPopup = true;
          this.extraPayChanged = false;
          this.requestSender.loadExtraPay(this.selectedCardId);
          this.requestSender.loadSolutions(this.cardId);
          return true;
        }).catch((resp: AxiosError) => {
          this.requestResult.success = false;
          this.requestResult.message = this.parseError(resp, 'extra_pay');
          this.requestResult.showPopup = true;
          return false;
        });
    },
    addWorkInfo() {
      this.workPopupTitle = 'Добавить место работы';
      this.showWorkPopup = true;
      this.editWork = new WorkInfo({});
    },
    editWorkInfo(work: WorkInfo) {
      this.workPopupTitle = 'Редактировать место работы';
      this.showWorkPopup = true;
      this.editWork = work;
    },
    //
    sendWorkInfo() {
      let request: Promise<any>;
      if (this.editWork.id) {
        request = this.requestSender.updateWorkInfo(this.card.id, this.editWork.toSendModel());
      } else {
        request = this.requestSender.createWorkInfo(this.card.id, this.editWork.toSendModel());
      }
      request
        .then((resp: AxiosResponse) => {
          this.requestResult.success = true;
          this.requestResult.message =
            'Стаж успешно сохранен' + (this.solutions.length ? this.needRecountStageMessage : '');
          this.requestResult.showPopup = true;
          this.requestSender.loadWorkInfo(this.card.id);
        }).catch((resp: AxiosError) => {
          this.requestResult.success = false;
          this.requestResult.message = resp.message;
          this.requestResult.showPopup = true;
        });
      this.showWorkPopup = false;
    },
    deleteWorkInfo(work: WorkInfo) {
      this.requestSender.deleteWorkInfo(work.id)
        .then((resp: AxiosResponse) => {
          this.requestResult.success = true;
          this.requestResult.message =
            'Место работы успешно удалено' + (this.solutions.length ? this.needRecountStageMessage : '');
          this.requestResult.showPopup = true;
          this.requestSender.loadWorkInfo(this.card.id);
        }).catch((resp: AxiosError) => {
          this.requestResult.success = false;
          this.requestResult.message = resp.message;
          this.requestResult.showPopup = true;
        });
    },
    approveWorkInfo() {
      const sendData = {
        personInfoId: this.card.id,
        approved: this.stageApproved,
        docsSubmitDate: this.docsSubmitDate,
        docsDestinationDate: this.docsDestinationDate,
      };
      this.requestSender.approveWorkInfos(sendData)
        .then((resp: AxiosResponse) => {
          this.requestResult.success = true;
          this.requestResult.message = 'Данные успешно сохранены';
          this.requestResult.showPopup = true;
          this.showWorkApprovePopup = false;
          // this.card.worksApproved = this.stageApproved;
          // this.card.docsSubmitDate = this.docsSubmitDate;
          // this.card.docsDestinationDate = this.docsDestinationDate;
          this.requestSender.loadWorkInfo(this.card.id);
        }).catch((resp: AxiosError) => {
          this.requestResult.success = false;
          this.requestResult.message = resp.message;
          this.requestResult.showPopup = true;
        });
    },
    editComment(payout: any) {
      if (true || payout.comment) {
        axios.post('/api/payout/comment', { payoutId: payout.id, comment: payout.comment }).then(() => {
          this.commentSaved = payout.id;
          setTimeout(() => this.commentSaved = '', 500);
        });
      }
    },
    cancel() {
      setTimeout(() => {
        let path = '/cards?';
        if (this.selectedCardId) {
          path += 'cardId=' + (this.selectedCardId || '') + '&';
        }
        path += 'tab=' + this.selectedTab;
        this.$router.push(path);
      }, 0);
    },
    sendSolution() {
      if (this.extraPayChanged && this.selectedTab === 'extra_pay') {
        this.showSolutionPopup = false;
        return this.saveExtraPay();
      }

      let url: string;
      switch (this.newSolution.type) {
        case 1:
          url = this.solutions.length
            ? '/api/solutions/count'
            : '/api/solutions/opredelit';
          break;
        case 2:
          url = '/api/solutions/pause';
          break;
        case 3:
          url = '/api/solutions/resume';
          break;
        case 4:
          url = '/api/solutions/stop';
          break;
        default:
          return;
      }

      axios.post(url, {
        personInfoRootId: this.selectedCardId,
        destination: this.newSolution.destination,
        execution: this.newSolution.execution,
        comment: this.newSolution.comment,
      }).then(() => {
        this.requestSender.loadSolutions(this.cardId);

        this.showSolutionPopup = false;


        this.newSolution = {
          type: -1,
          createSolution: true,
          destination: formatDate(new Date().toISOString()),
          execution: '',
          comment: '',
        };
      });
    },
    editSolutionComment(solution: any) {
      this.showEditSolutionCommentPopup = true;
      this.solutionForEdit = solution;
      this.newSolutionComment = solution.comment;
    },
    acceptSolutionComment() {
      axios.post('/api/solutions/edit_comment', { id: this.solutionForEdit.id, comment: this.newSolutionComment })
        .then(() => {
          this.solutionForEdit.comment = this.newSolutionComment;
          this.showEditSolutionCommentPopup = false;
        });
    },
    deleteSolution(solution: any) {
      this.solutionForDeleteId = solution.id;
      this.showConfirmDeleteSolutionPopup = true;
    },
    confirmDeleteSolution() {
      if (this.solutionForDeleteId) {
        axios.post('/api/solutions/delete', { id: this.solutionForDeleteId })
          .then(() => {
            this.requestSender.loadSolutions(this.cardId);
          })
          .catch((resp: AxiosError) => {
            this.requestResult.success = false;
            this.requestResult.message = resp.response ? resp.response.data : resp.message;
            this.requestResult.showPopup = true;
          });
      }
      this.showConfirmDeleteSolutionPopup = false;
    },
    editBankCard() {
      this.requestSender.createPersonBankCard({
        personRootId: this.selectedCardId,
        id: this.newBankCard.id,
        type: this.newBankCardType,
        number: this.newBankCard.number || this.newBankCard.account,
        validThru: this.newBankCard.validThru,
      })
        .then(() => {
          this.showBankCardPopup = false;
          this.requestSender.getPersonBankCard(this.selectedCardId);
        });
    },
    formatDate(date: string): string {
      return formatDate(date);
    },
    formatInputMoney(e: any) {
      if (e && e.target) {
        e.target.value = formatMoney(e.target.value);
      }
    },
    calcAllPercsBySumm() {
      this.deactivateWatcher('secrecyDivPerc');
      this.deactivateWatcher('qualificationDivPerc');
      this.deactivateWatcher('premiumPerc');
      this.deactivateWatcher('perksDivPerc');
      this.deactivateWatcher('vyslugaDivPerc');

      this.secrecyDivPerc = formatMoney(calculatePercents(this.salaryMultiplied, this.secrecy), 0);
      this.qualificationDivPerc = formatMoney(calculatePercents(this.salaryMultiplied, this.qualification), 2);
      this.premiumPerc = formatMoney(calculatePercents(this.salaryMultiplied, this.premium), 0);
      this.perksDivPerc = formatMoney(calculatePercents(this.salaryMultiplied, this.perks), 0);
      this.vyslugaDivPerc = formatMoney(calculatePercents(this.salaryMultiplied, this.vysluga), 0);

      this.extraPay.secrecyDivPerc = this.secrecyDivPerc;
      this.extraPay.qualificationDivPerc = this.qualificationDivPerc;
      this.extraPay.premiumPerc = this.premiumPerc;
      this.extraPay.perksDivPerc = this.perksDivPerc;
      this.extraPay.vyslugaDivPerc = this.vyslugaDivPerc;

      this.activateWatcherAsync('secrecyDivPerc');
      this.activateWatcherAsync('qualificationDivPerc');
      this.activateWatcherAsync('premiumPerc');
      this.activateWatcherAsync('perksDivPerc');
      this.activateWatcherAsync('vyslugaDivPerc');
    },
    calcAllSummByPercs() {
      this.deactivateWatcher('perks');
      this.deactivateWatcher('premium');
      this.deactivateWatcher('qualification');
      this.deactivateWatcher('secrecy');
      this.deactivateWatcher('vysluga');

      this.perks = formatMoney(calculateSumm(this.salaryMultiplied, this.perksDivPerc));
      this.premium = formatMoney(calculateSumm(this.salaryMultiplied, this.premiumPerc));
      this.qualification = formatMoney(calculateSumm(this.salaryMultiplied, this.qualificationDivPerc));
      this.secrecy = formatMoney(calculateSumm(this.salaryMultiplied, this.secrecyDivPerc));
      this.vysluga = formatMoney(calculateSumm(this.salaryMultiplied, this.vyslugaDivPerc));

      this.extraPay.perks = this.perks;
      this.extraPay.premium = this.premium;
      this.extraPay.qualification = this.qualification;
      this.extraPay.secrecy = this.secrecy;
      this.extraPay.vysluga = this.vysluga;

      this.activateWatcherAsync('perks');
      this.activateWatcherAsync('premium');
      this.activateWatcherAsync('qualification');
      this.activateWatcherAsync('secrecy');
      this.activateWatcherAsync('vysluga');
    },
    activateWatcherAsync(name: string) {
      setTimeout(() => {
        const watcher = (this as any)._watchers.find((el: any) => el.expression === name);
        if (watcher) {
          watcher.active = true;
        }
      }, 0);
    },
    deactivateWatcher(name: string) {
      const watcher = (this as any)._watchers.find((el: any) => el.expression === name);
      if (watcher) {
        watcher.active = false;
      }
    },
    activateAllWatchersAsync() {
      setTimeout(() => {
        (this as any)._watchers.forEach((el: any) => el.active = true);
      }, 0);
    },
    deactivateAllWatchers() {
      (this as any)._watchers.forEach((el: any) => el.active = false);
    },
    activateExtraPayWatchersAsync() {
      setTimeout(() => {
        (this as any)._watchers.forEach((el: any) => {
          if (this.extraPayWatchers.includes(el.expression)) {
            el.active = true;
          }
        });
      }, 100);
    },
    deactivateExtraPayWatchers() {
      (this as any)._watchers.forEach((el: any) => {
        if (this.extraPayWatchers.includes(el.expression)) {
          el.active = false;
        }
      });
    },
    sendSnilsRequest() {
      this.requestSender.sendSnilsRequest(this.selectedCardId);
    },
  },
  components: {
    CommonButton,
    BubbleButton,
    CrossButton,
    SaveButton,
    EditButton,
    PlusButton,
    SymbolCButton,
    CheckBox,
    LabeledInput,
    LabeledInputWithPrompt,
    Scrollable,
    LabeledBlock,
    DropDown,
    Popup,
    WorkInfoPopup,
    RequestResultPopup,
  },
});
</script>
