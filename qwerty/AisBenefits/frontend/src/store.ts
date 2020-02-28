import Vue from 'vue';
import Vuex from 'vuex';
import axios, { AxiosResponse } from '../node_modules/axios';
import PersonInfo from './models/person/personInfo';
import CardEditData from './models/cardEditData';
import Solution from './models/person/solution';
import RequestSender from './requestSender';
import { formatMoney } from './models/utils/stringBuilder';
import User from './models/users/user';
import EgissoPeriod from './models/egisso/egissoPeriod';
import MeasureUnit from './models/egisso/measureUnit';
import EgissoForm from './models/egisso/egissoForm';
import RecipientCode from './models/egisso/recipientCode';
import SocialHelp from './models/egisso/socialHelp';

Vue.use(Vuex);

export default new Vuex.Store({
  state: {
    selectedCardId: '',
    selectedCard: new PersonInfo(),
    userId: '',
    user: {} as User,
    usertoken: '',
    users: [] as User[],
    cards: [{}],
    cardEditData: new CardEditData(),
    loadersCount: 0,
    functionsList: [],
    organizationsList: [],
    mdsPercentsList: [],
    minExtraPay: '',
    extraPayVariants: [],
    rolesList: [],
    permissionsList: [] as Array<{}>,
    requestSender: {} as RequestSender,
    egissoPeriods: [] as EgissoPeriod[],
    measureUnits: [] as MeasureUnit[],
    egissoForms: [] as EgissoForm[],
    recipientCodes: [] as RecipientCode[],
    socialHelps: [] as SocialHelp[],
  },
  getters: {
    requestSender: (state): RequestSender => {
      return state.requestSender;
    },
    isAuthorized: (state): boolean => {
      return state.userId ? true : false;
    },
    isCardSelected: (state): boolean => {
      return (state.selectedCardId) ? true : false;
    },
    selectedCardId: (state): string => {
      return state.selectedCardId;
    },
    userName: (state): string => {
      if (!state.user) {
        return '';
      }
      return (state.user.name + (
        (state.user.secondName && state.user.secondName.length) ? ' ' + state.user.secondName[0] : ''));
    },
    userFullName: (state): string => {
      return (state.user.name + (state.user.secondName ? ' ' + state.user.secondName : ''));
    },
    userToken: (state): string => {
      return state.usertoken;
    },
    usersList: (state) => {
      return state.users;
    },
    cardList: (state): object[] => {
      return state.cards;
    },
    selectedCard: (state): PersonInfo => {
      return state.selectedCard;
    },
    isLoading: (state): boolean => {
      return state.loadersCount > 0;
    },
    cardEditData: (state): CardEditData => {
      return state.cardEditData;
    },
    functionsList: (state) => {
      return state.functionsList;
    },
    organizationsList: (state) => {
      return state.organizationsList;
    },
    mdsPercentsList: (state) => {
      return state.mdsPercentsList;
    },
    minExtraPay: (state) => {
      return state.minExtraPay;
    },
    extraPayVariants: (state) => {
      return state.extraPayVariants;
    },
    rolesList: (state) => {
      return state.rolesList;
    },
    permissionsList: (state) => {
      return state.permissionsList;
    },
    egissoPeriods: (state) => {
      return state.egissoPeriods;
    },
    measureUnits: (state) => {
      return state.measureUnits;
    },
    egissoForms: (state) => {
      return state.egissoForms;
    },
    recipientCodes: (state) => {
      return state.recipientCodes;
    },
    socialHelps: (state) => {
      return state.socialHelps;
    },
  },
  mutations: {
    setRequestSender(state, sender: RequestSender) {
      state.requestSender = sender;
    },
    setUser(state, userData) {
      state.user = userData;
    },
    setUsersList(state, users) {
      state.users = users;
    },
    setToken(state, token: string) {
      state.usertoken = token;
    },
    setUserId(state, userId: string) {
      state.userId = userId;
    },
    setSelectedCardId: (state, cardId: string) => {
      state.selectedCardId = cardId;
    },
    setSelectedCard: (state, card: PersonInfo) => {
      state.selectedCard = card;
    },
    setCardList: (state, cardList: Array<{ number: number }>) => {
      state.cards = cardList.sort((a, b) => {
        const diff = a.number - b.number;
        if (diff > 0) {
          return -1;
        } else if (diff < 0) {
          return 1;
        }
        return 0;
      });
    },
    setCardEditData: (state, data: CardEditData) => {
      state.cardEditData = data;
    },
    addLoader: (state) => {
      state.loadersCount += 1;
    },
    removeLoader: (state) => {
      state.loadersCount -= 1;
    },
    setFunctionsList: (state, data) => {
      state.functionsList = data;
    },
    setOrganizationsList: (state, data) => {
      state.organizationsList = data;
    },
    setMdsPercentsList: (state, data) => {
      state.mdsPercentsList = data;
    },
    setMinExtraPay: (state, value) => {
      state.minExtraPay = formatMoney(value);
    },
    setExtraPayVariants: (state, data) => {
      state.extraPayVariants = data;
    },
    setRolesList: (state, data) => {
      state.rolesList = data;
    },
    setPermissionsList: (state, data) => {
      state.permissionsList = data;
    },
    setSelectedCardWorkInfo: (state, data) => {
      state.selectedCard.works = data.works;
      state.selectedCard.worksApproved = data.approved;
      state.selectedCard.docsSubmitDate = data.docsSubmitDate;
      state.selectedCard.docsDestinationDate = data.docsDestinationDate;
      state.selectedCard.setWorkAge(data.ageYears, data.ageMonths, data.ageDays);
    },
    setSelectedCardExtraPay: (state, data) => {
      state.selectedCard.extraPay = data;
    },
    setSelectedCardPayouts: (state, data) => {
      state.selectedCard.payouts = data;
    },
    setSelectedCardPersonBankCard: (state, data) => {
      state.selectedCard.bankCard = data;
    },
    setSelectedCardSolutions: (state, data) => {
      state.selectedCard.solutions = data.map((el: any) => new Solution(el));
    },
    setEgissoPeriods: (state, data) => {
      state.egissoPeriods = data.map((el: any) => new EgissoPeriod(el));
    },
    setMeasureUnits: (state, data) => {
      state.measureUnits = data.map((el: any) => new MeasureUnit(el));
    },
    setEgissoForms: (state, data) => {
      state.egissoForms = data.map((el: any) => new EgissoForm(el));
    },
    setRecipientCodes: (state, data) => {
      state.recipientCodes = data.map((el: any) => new RecipientCode(el));
    },
    setSocialHelps: (state, data) => {
      state.socialHelps = data.map((el: any) => new SocialHelp(el));
    },
  },
  actions: {
    loadUserDataByToken(context) {
      const cookieName = 'Authorization=';
      const authCookie = (document.cookie.split(/[; ]/).find((e) => e.indexOf(cookieName) >= 0) || '')
        .substr(cookieName.length, 32);
      if (!authCookie) {
        return;
      }
      const userId = authCookie.substring(0, 8) + '-' +
        authCookie.substring(8, 12) + '-' +
        authCookie.substring(12, 16) + '-' +
        authCookie.substring(16, 20) + '-' +
        authCookie.substring(20, 32);

      if (!context.getters.isAuthorized) {
        context.commit('setUserId', userId);
      }

      context.getters.requestSender.getUserData(userId);
    },
    log_out(context) {
      context.commit('setUser', null);
      context.commit('setUserId', '');
    },
  },
});
