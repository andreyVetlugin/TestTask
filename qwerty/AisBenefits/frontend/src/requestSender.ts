import axios, { AxiosPromise, AxiosResponse, AxiosError } from 'axios';
import { Store } from 'vuex';
import PersonBankCard from './models/person/personBankCard';
import PersonInfo from './models/person/personInfo';
import User from './models/users/user';
import WorkInfo from './models/person/workInfo';
import { formatDate } from './models/utils/stringBuilder';
import ExtraPay from './models/person/extraPayInfo';
import Payout from './models/person/payout';
import CardEditData from './models/cardEditData';
import moment from 'moment';

export default class RequestSender {

    public requestResult = {
        success: true,
        message: '',
        showPopup: false,
    };

    private urls = {
        personInfo: '/api/personinfo/',
        registry: '/api/reestr/',
        gosPension: '/api/GosPensionUpdate/',
        workInfo: '/api/WorkInfo/',
        payout: '/api/Payout/',
        solution: '/api/solutions/',
        bankCard: '/api/PersonBankCard/',
        minExtraPay: '/api/MinExtraPay/',
        extraPay: '/api/extrapay/',
        personInfoRecount: '/api/Recount/',
        extraPayVariants: '/api/extrapayvariant/',
        massRecount: '/api/massrecalculate/',
        user: '/api/users/',
        role: '/api/roles/',
        сounters: '/api/Counters/',
        excelReport: '/api/ExcelReport/',
        egisso: '/api/',
    };

    private docxMime = 'application/vnd.openxmlformats-officedocument.wordprocessingml.document';
    private xlsxMime = 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet';
    private context: Store<any>;

    private readonly defaultErrorMessage = 'Ошибка при отправке запроса.';

    constructor(context: Store<any>) {
        this.context = context;
    }

    public auth(login: string, password: string): Promise<any> {
        return this.send(RequestTypes.post, '/api/authorize/auth', { login, password })
            .then((resp: AxiosResponse) => {
                const userId = resp.data.userId;
                if (userId) {
                    sessionStorage.setItem('userId', userId);
                    this.context.dispatch('loadUserDataByToken');
                    this.context.commit('setUserId', userId);
                }
            });
    }

    public getUserData(id: string): Promise<User> {
        return this.getUser(id).then((user) => {
            this.context.commit('setUser', user);
            return user;
        });
    }

    public getMenuCounters() {
        return this.send(RequestTypes.get, this.urls.сounters + 'menu_counters');
    }

    public buildReport(report: any) {
        try {
            report = report.toServerModel();
        } catch (err) {
            this.showPopup(err, false);
            return null;
        }
        return this.send(RequestTypes.postArrayBuffer, this.urls.excelReport + 'BuildExcelReport', report)
            .then((resp: AxiosResponse) => {
                const fileDownload = require('js-file-download');
                fileDownload(resp.data,
                    `Сводный отчёт от ${formatDate(new Date().toISOString())}.xlsx`,
                    this.xlsxMime);
            });
    }

    //#region PersonInfo

    public searchCards(FIO?: string, pageNumber?: number): Promise<any> {
        const sendData = {
            FIO,
            pageNumber,
        };
        return this.send(RequestTypes.post, this.urls.personInfo + 'search', sendData)
            .then((resp: AxiosResponse) => {
                const cards = (resp.data.result as any[]).map((el) => new PersonInfo(el));
                this.context.commit('setCardList', cards);
                return resp;
            });
    }

    public searchArchiveCards(FIO?: string, pageNumber?: number): Promise<any> {
        const sendData = {
            FIO,
            pageNumber,
        };
        return this.send(RequestTypes.post, this.urls.personInfo + 'SearchArchive', sendData)
            .then((resp: AxiosResponse) => {
                const cards = (resp.data.result as any[]).map((el) => new PersonInfo(el));
                this.context.commit('setCardList', cards);
                return resp;
            });
    }

    public loadCard(id: string) {
        return this.send(RequestTypes.post, this.urls.personInfo + 'get', { id }, false)
            .then((resp: AxiosResponse) => {
                this.context.commit('setSelectedCard', new PersonInfo(resp.data));
                return Promise.all([
                    this.loadWorkInfo(id),
                    this.loadExtraPay(id),
                    this.loadPayouts(id),
                ]).then(() => resp);
            });
    }

    public createEditCard(card: PersonInfo) {
        const url = this.urls.personInfo + (card.id ? 'edit' : 'create');
        return this.send(RequestTypes.post, url, card.toServerModel(), false)
            .then((resp: AxiosResponse) => {
                if (!card.id) {
                    this.context.commit('setSelectedCardId', resp.data.personRootId);
                }
                return resp;
            });
    }

    public loadCardEditData() {
        return this.send(RequestTypes.get, this.urls.personInfo + 'getconstantlist')
            .then((resp: AxiosResponse) => {
                this.context.commit('setCardEditData', new CardEditData(resp.data));
            });
    }

    public sendSnilsRequest(id: string) {
        return this.send(RequestTypes.post, this.urls.personInfo + 'snilsrequest', { id }, true)
            .then((resp: AxiosResponse) => {
                this.requestResult.message = 'запрос принят в обработку';
                this.requestResult.success = true;
                this.requestResult.showPopup = true;
            });
    }

    public loadDeadmenFile(data: FormData) {
        return this.send(RequestTypes.postFile, '/api/zags/upload', data, true);
    }

    //#region ExtraPay

    public loadExtraPay(id: string) {
        return this.send(RequestTypes.post, this.urls.extraPay + 'get', { personRootId: id }, false)
            .then((resp: AxiosResponse) => {
                this.context.commit('setSelectedCardExtraPay', new ExtraPay(resp.data));
            });
    }

    public saveExtraPay(data: any) {
        return this.send(RequestTypes.post, '/api/extrapay/edit', data, false);
    }

    //#endregion

    //#region Recount

    public personInfoGetRecount(data: any): Promise<any> {
        return this.send(RequestTypes.post, this.urls.personInfoRecount + 'get', data);
    }

    public personInfoConfirmRecount(data: any): Promise<any> {
        return this.send(RequestTypes.post, this.urls.personInfoRecount + 'confirm', data);
    }

    //#endregion

    //#region Payouts

    public loadPayouts(personId: string) {
        return this.send(RequestTypes.post, this.urls.payout + 'getall', { personId }, false)
            .then((resp: AxiosResponse) => {
                const payouts = [];
                for (const payout of resp.data) {
                    payouts.push(new Payout(payout));
                }
                this.context.commit('setSelectedCardPayouts', payouts);
            });
    }

    //#endregion

    //#region Solutions

    public loadSolutions(id: string) {
        return this.send(RequestTypes.post, this.urls.solution + 'get', { id }, false)
            .then((resp: AxiosResponse) => {
                this.context.commit('setSelectedCardSolutions', resp.data);
            });
    }

    //#endregion

    //#endregion

    //#region Реестр

    public getRegistry(): Promise<any> {
        return this.send(RequestTypes.get, this.urls.registry + 'get');
    }
    // todo force new registry link
    public getForceNewRegistry(registry: any): Promise<any> {
        return this.send(RequestTypes.post, this.urls.registry + 'init', registry);
    }
    public getRegistryArchive(year: number, month: number): Promise<any> {
        return this.send(RequestTypes.post, this.urls.registry + 'getArchive', { year, month });
    }

    public getRegistryReestrElements(id: string): Promise<any> {
        return this.send(RequestTypes.post, this.urls.registry + 'getReestrElements', { id });
    }
    public completeRegistry(id: string, date: string, forSignature: boolean): Promise<any> {
        return this.send(RequestTypes.post, this.urls.registry + 'complete', { id, forSignature })
            .then((resp: AxiosResponse) => {
                const fileDownload = require('js-file-download');
                const iconv = require('iconv-lite');
                fileDownload(iconv.encode(resp.data, 'win1251'), 'реестр ' + date + '.txt', 'application/msword');
            });
    }
    public deleteRegistryElement(id: string): Promise<any> {
        return this.send(RequestTypes.post, this.urls.registry + 'deleteElement', { id });
    }
    public reCountRegistryElement(form: any): Promise<any> {
        return this.send(RequestTypes.post, this.urls.registry + 'reCountElement', form);
    }
    public reCountAllRegistryElements(recountReestrElementForms: any[]): Promise<any> {
        return this.send(RequestTypes.post, this.urls.registry + 'reCountAllElements', { recountReestrElementForms });
    }

    //#endregion Реестр

    //#region GosPension

    public approveGosPension(
        gosPensionUpdateIds: string[],
        destination: string,
        execution: string,
        comment: string,
    ): Promise<any> {
        const data = {
            gosPensionUpdateIds,
            destination,
            execution,
            comment,
        };
        return this.send(RequestTypes.post, this.urls.gosPension + 'approve', data);
    }
    public declineGosPension(ids: string[]): Promise<any> {
        const data = {
            declinedPensionUpdates: ids.map((incomePensionId) => {
                return { incomePensionId };
            }),
        };
        return this.send(RequestTypes.post, this.urls.gosPension + 'decline', data);
    }
    public syncGosPension(id: string): Promise<any> {
        return this.send(RequestTypes.post, this.urls.gosPension + 'syncpfr', { id })
            .catch((resp: AxiosError) => {
                const message = (resp.response && typeof resp.response.data === 'string')
                    ? resp.response.data
                    : 'Ошибка отправки запроса. Попробуйте повторить позже.';
                this.showPopup(message, false);
                throw resp;
            });
    }
    public syncAllGosPension(): Promise<any> {
        return this.send(RequestTypes.post, this.urls.gosPension + 'syncpfrall');
    }
    public getPensionUpdateRows(): Promise<any> {
        return this.send(RequestTypes.post, this.urls.gosPension + 'get');
    }

    //#endregion

    //#region WorkInfo

    public loadWorkInfo(id: string) {
        return this.send(RequestTypes.post, this.urls.workInfo + 'getWorkInfo', { id }, false)
            .then((resp: AxiosResponse) => {
                const wiData = {
                    ageDays: resp.data.ageDays,
                    ageMonths: resp.data.ageMonths,
                    ageYears: resp.data.ageYears,
                    approved: resp.data.approved,
                    docsSubmitDate: formatDate(resp.data.docsSubmitDate),
                    docsDestinationDate: formatDate(resp.data.docsDestinationDate),
                    works: [] as WorkInfo[],
                };
                for (const work of resp.data.workPlaces) {
                    wiData.works.push(new WorkInfo(work));
                }
                this.context.commit('setSelectedCardWorkInfo', wiData);
            });
    }

    public createWorkInfo(personId: string, data: any) {
        const sendData = {
            personInfoId: personId,
            workPlace: data,
        };
        return this.send(RequestTypes.post, this.urls.workInfo + 'createOneWorkInfo', sendData, false);
    }
    public updateWorkInfo(personId: string, data: any) {
        const sendData = {
            personInfoId: personId,
            workPlace: data,
        };
        return this.send(RequestTypes.post, this.urls.workInfo + 'UpdateOneWorkInfo', sendData, false);
    }
    public deleteWorkInfo(id: string) {
        return this.send(RequestTypes.post, this.urls.workInfo + 'DeleteOneWorkInfo', { id }, false);
    }
    public approveWorkInfos(sendData: any) {
        return this.send(RequestTypes.post, this.urls.workInfo + 'ConfirmExperience', sendData, false);
    }

    //#endregion

    //#region BankCard

    public getPersonBankCard(id: string): Promise<any> {
        return this.send(RequestTypes.post, this.urls.bankCard + 'Get', { id }, false)
            .then((resp: AxiosResponse) => {
                this.context.commit('setSelectedCardPersonBankCard', new PersonBankCard(resp.data));
            })
            .catch((resp: AxiosError) => {
                if (resp.message !== 'Указанный банковский счет не существует') {
                    return resp;
                }
            });
    }

    public createPersonBankCard(cardData: any): Promise<any> {
        const url = this.urls.bankCard +
            (cardData.id && cardData.id !== '00000000-0000-0000-0000-000000000000' ? 'update' : 'create');
        return this.send(RequestTypes.post, url, cardData);
    }

    //#endregion

    //#region Config

    //#region Functions

    public getAllFunctions() {
        return this.send(RequestTypes.get, '/api/functions/getall')
            .then((resp: AxiosResponse) => {
                this.context.commit('setFunctionsList', resp.data);
            });
    }

    public createFunction(data: any) {
        return this.send(RequestTypes.post, '/api/functions/create', data)
            .then((resp: AxiosResponse) => {
                this.getAllFunctions();
            });
    }

    public editFunction(data: any) {
        return this.send(RequestTypes.post, '/api/functions/edit', data)
            .then((resp: AxiosResponse) => {
                this.getAllFunctions();
            });
    }

    public removeFunction(functionId: string): Promise<any> {
        return this.send(RequestTypes.post, '/api/functions/delete', { functionId }, false)
            .then(() => {
                this.getAllFunctions();
            })
            .catch((resp: AxiosError) => {
                this.showErrorPopup(resp);
            });
    }

    //#endregion

    //#region Organizations

    public getAllOrganizations() {
        return this.send(RequestTypes.get, '/api/organizations/getall')
            .then((resp: AxiosResponse) => {
                this.context.commit('setOrganizationsList', resp.data);
            });
    }

    public createOrganization(data: any) {
        return this.send(RequestTypes.post, '/api/organizations/create', data)
            .then((resp: AxiosResponse) => {
                this.getAllOrganizations();
            });
    }

    public editOrganization(data: any) {
        return this.send(RequestTypes.post, '/api/organizations/edit', data)
            .then((resp: AxiosResponse) => {
                this.getAllOrganizations();
            });
    }

    public removeOrganization(id: string): Promise<any> {
        return this.send(RequestTypes.post, '/api/organizations/delete', { id }, false)
            .then(() => {
                this.getAllOrganizations();
            })
            .catch((resp: AxiosError) => {
                this.showErrorPopup(resp);
            });
    }

    //#endregion

    //#region MdsPercents

    public getAllMdsPercents() {
        return this.send(RequestTypes.get, '/api/dsperc/getall')
            .then((resp: AxiosResponse) => {
                this.context.commit('setMdsPercentsList', resp.data.map((el: any) => {
                    return {
                        year: el.year,
                        dsPercs: el.dsPercs,
                    };
                }));
            });
    }

    public updateMdsPercent(data: any): Promise<any> {
        return this.send(RequestTypes.post, '/api/dsperc/edityear', data, false)
            .then((resp: AxiosResponse) => {
                this.getAllMdsPercents();
            })
            .catch((resp: AxiosError) => {
                this.showErrorPopup(resp);
            });
    }

    public removeMdsPercent(id: string): Promise<any> {
        return this.send(RequestTypes.post, '/api/dsperc/delete', { id }, false)
            .then(() => {
                this.getAllMdsPercents();
            })
            .catch((resp: AxiosError) => {
                this.showErrorPopup(resp);
            });
    }

    //#endregion

    //#region ExtraPayVariants

    public getAllExtraPayVariants() {
        return this.send(RequestTypes.get, this.urls.extraPayVariants + 'getall')
            .then((resp: AxiosResponse) => {
                this.context.commit('setExtraPayVariants', resp.data);
            });
    }

    public createExtraPayVariant(variant: any) {
        const url = this.urls.extraPayVariants + (variant.id ? 'edit' : 'create');
        return this.send(RequestTypes.post, url, {
            variantId: variant.id,
            number: variant.number || 0,
            uralMultiplier: variant.uralMul || null,
            premiumPerc: variant.premPerc || null,
            matSupportMultiplier: variant.matSupMul || null,
            vyslugaMultiplier: variant.vysMul || null,
            vyslugaDivPerc: variant.vysDivPerc || null,
            ignoreGosPension: variant.ignoreGosPension === 'true',
        }, false)
            .then(() => {
                this.getAllExtraPayVariants();
            })
            .catch((resp: AxiosError) => {
                this.showErrorPopup(resp);
            });
    }

    public removeExtraPayVariants(id: string): Promise<any> {
        return this.send(RequestTypes.post, this.urls.extraPayVariants + 'delete', { id }, false)
            .then(() => {
                this.getAllExtraPayVariants();
            })
            .catch((resp: AxiosError) => {
                this.showErrorPopup(resp);
            });
    }

    //#endregion

    //#region MinExtraPay

    public getMinExtraPay() {
        return this.send(RequestTypes.get, this.urls.minExtraPay + 'get', {}, false)
            .then((resp: AxiosResponse) => {
                this.context.commit('setMinExtraPay', resp.data.value);
            })
            .catch((resp: AxiosError) => {
                this.showErrorPopup(resp);
            });
    }
    public setMinExtraPay(value: number) {
        return this.send(RequestTypes.post, this.urls.minExtraPay + 'edit', { value }, false)
            .then(() => {
                this.context.commit('setMinExtraPay', value);
            })
            .catch((resp: AxiosError) => {
                this.showErrorPopup(resp);
            });
    }

    //#endregion

    //#endregion

    //#region Roles

    public getUser(id: string): Promise<User> {
        return this.send(RequestTypes.post, this.urls.user + 'get', { id })
            .then((resp: AxiosResponse) => {
                return new User(resp.data);
            });
    }
    public getAllUsers() {
        return this.send(RequestTypes.get, this.urls.user + 'getall')
            .then((resp: AxiosResponse) => {
                this.context.commit('setUsersList', resp.data.map((el: any) => new User(el)));
            });
    }
    public createEditUser(user: User) {
        const url = this.urls.user + (user.id ? 'edit' : 'create');
        return this.send(RequestTypes.post, url, user.toServerModel(), false)
            .then((resp: AxiosResponse) => {
                this.getAllUsers();
                return resp;
            })
            .catch((resp: AxiosError) => {
                this.showErrorPopup(resp);
                throw resp;
            });
    }

    public getAllRoles() {
        return this.send(RequestTypes.get, this.urls.role + 'getall')
            .then((resp: AxiosResponse) => {
                this.context.commit('setRolesList', resp.data);
            });
    }

    public createEditRole(role: any) {
        const url = this.urls.role + (role.id ? 'edit' : 'create');
        return this.send(RequestTypes.post, url, role, false)
            .then(() => {
                this.getAllRoles();
            })
            .catch((resp: AxiosError) => {
                this.showErrorPopup(resp);
                throw resp;
            });
    }

    public getAllPermissions() {
        return this.send(RequestTypes.get, this.urls.role + 'getpermissions')
            .then((resp: AxiosResponse) => {
                this.context.commit('setPermissionsList', resp.data);
            });
    }

    //#endregion

    //#region Egisso

    public getEgissoPeriods() {
        return this.send(RequestTypes.get, '/api/EgissoPeriodType/getAll').then((resp: AxiosResponse) => {
            this.context.commit('setEgissoPeriods', resp.data);
        });
    }

    public createEditEgissoPeriod(period: any) {
        const url = '/api/EgissoPeriodType/' + (period.id ? 'Edit' : 'Create');
        return this.send(RequestTypes.post, url, period.toServerModel())
            .then(() => this.getEgissoPeriods());
    }

    public getMeasureUnits() {
        return this.send(RequestTypes.get, '/api/EgissoMeasureUnit/getAll').then((resp: AxiosResponse) => {
            this.context.commit('setMeasureUnits', resp.data);
        });
    }

    public createEditMeasureUnit(unit: any) {
        const url = '/api/EgissoMeasureUnit/' + (unit.id ? 'Edit' : 'Create');
        return this.send(RequestTypes.post, url, unit.toServerModel())
            .then(() => this.getMeasureUnits());
    }

    public getEgissoForms() {
        return this.send(RequestTypes.get, '/api/EgissoProvisionForm/getAll').then((resp: AxiosResponse) => {
            this.context.commit('setEgissoForms', resp.data);
        });
    }

    public createEditEgissoForm(form: any) {
        const url = '/api/EgissoProvisionForm/' + (form.id ? 'Edit' : 'Create');
        return this.send(RequestTypes.post, url, form.toServerModel())
            .then(() => this.getEgissoForms());
    }

    public getRecipientCodes() {
        return this.send(RequestTypes.get, '/api/EgissoKpCode/getAll').then((resp: AxiosResponse) => {
            this.context.commit('setRecipientCodes', resp.data);
        });
    }

    public createEditRecipientCode(code: any) {
        const url = '/api/EgissoKpCode/' + (code.id ? 'Edit' : 'Create');
        return this.send(RequestTypes.post, url, code.toServerModel())
            .then(() => this.getRecipientCodes());
    }

    public getSocialHelp() {
        return this.send(RequestTypes.get, '/api/EgissoPrivilege/getAll').then((resp: AxiosResponse) => {
            this.context.commit('setSocialHelps', resp.data);
        });
    }

    public createEditSocialHelp(socialHelp: any) {
        const url = '/api/EgissoPrivilege/' + (socialHelp.id ? 'Edit' : 'Create');
        return this.send(RequestTypes.post, url, socialHelp.toServerModel())
            .then(() => this.getSocialHelp());
    }

    public printEgissoFacts(destinationDate: string, from: string, to: string) {
        return this.send(RequestTypes.postArrayBuffer, '/api/egisso/fact', { destinationDate, from, to })
            .then((resp: AxiosResponse) => {
                const fileDownload = require('js-file-download');
                const filename = `Выгрузка фактов Из ЕГИССО - назначение от ${destinationDate} с ${from} по ${to}.zip`;
                fileDownload(resp.data, filename);
            });
    }

    public printEgissoValidation() {
        return this.send(RequestTypes.postArrayBuffer, '/api/egisso/validation')
            .then((resp: AxiosResponse) => {
                const fileDownload = require('js-file-download');
                const filename = `Валидация ЕГИССО от ${moment().format('dd.MM.yyyy HH:mm')}.csv`;
                fileDownload(resp.data, filename);
            });
    }

    public getPrintHistory() {
        return this.send(RequestTypes.get, '/api/egisso/history');
    }

    //#endregion

    //#region Справки

    public printPersonInfo(id: string, fio: string, date: string) {
        return this.send(RequestTypes.postArrayBuffer, this.urls.personInfo + 'GetWordPrintDocument', { id })
            .then((resp: AxiosResponse) => {
                const fileDownload = require('js-file-download');
                fileDownload(resp.data, `Данные карты ${fio} ${date}.docx`, this.docxMime);
            });
    }

    public printWorkInfo(id: string, fio: string, date: string) {
        return this.send(RequestTypes.postArrayBuffer, this.urls.workInfo + 'GetExcel', { id })
            .then((resp: AxiosResponse) => {
                const fileDownload = require('js-file-download');
                fileDownload(resp.data, `Стаж ${fio} ${date}.xlsx`, this.xlsxMime);
            });
    }

    public printCertificateOfPensionSupplement(id: string, fio: string, from: string, to: string) {
        return this.send(RequestTypes.postArrayBuffer, this.urls.payout + 'GetWordPrintDocument', { id, from, to })
            .then((resp: AxiosResponse) => {
                const fileDownload = require('js-file-download');
                fileDownload(resp.data, `Справка о доплате к пенсии ${fio} ${from}-${to}.docx`, this.docxMime);
            });
    }

    public printSolution(id: string, filename: string) {
        return this.send(RequestTypes.postArrayBuffer, this.urls.solution + 'PrintSolution', { id })
            .then((resp: AxiosResponse) => {
                const fileDownload = require('js-file-download');
                fileDownload(resp.data, `${filename}.docx`, this.docxMime);
            });
    }

    public printSolutionCertificate(link: string, id: string, filename: string) {
        return this.send(RequestTypes.postArrayBuffer, this.urls.solution + link, { id })
            .then((resp: AxiosResponse) => {
                const fileDownload = require('js-file-download');
                fileDownload(resp.data, `${filename}.docx`, this.docxMime);
            });
    }

    //#endregion

    //#region MassRecount

    public massRecount(data: {}): Promise<any> {
        return this.send(RequestTypes.post, this.urls.massRecount + 'recalculate', data);
    }

    //#endregion

    private send(type: RequestTypes, url: string, data?: any, needCatch = true): AxiosPromise<any> {
        let result: AxiosPromise;

        this.context.commit('addLoader');

        switch (type) {
            case RequestTypes.postArrayBuffer:
                result = axios.post(url, data, { responseType: 'arraybuffer' });
                break;
            case RequestTypes.post:
                result = axios.post(url, data);
                break;
            case RequestTypes.postFile:
                result = axios.post(url, data, { headers: { 'Content-Type': 'multipart/form-data' }});
                break;
            case RequestTypes.get:
                result = axios.get(url, data);
                break;
            case RequestTypes.delete:
                result = axios.delete(url, data);
                break;
            case RequestTypes.head:
                result = axios.head(url, data);
                break;
            case RequestTypes.patch:
                result = axios.patch(url, data);
                break;
            case RequestTypes.put:
                result = axios.put(url, data);
                break;
            default:
                throw new Error('type not found');
        }
        return result
            .catch((resp: AxiosError) => {
                if (needCatch) {
                    this.showErrorPopup(resp);
                }
                throw resp;
            })
            .finally(() => {
                this.context.commit('removeLoader');
            });
    }

    private showErrorPopup(resp: AxiosError) {
        let message = '';
        if (resp.response) {
            const data = resp.response.data;
            if (data.message) {
                message = data.message;
            } else if (typeof data === 'string') {
                message = data;
            } else if (data.toString() === '[object ArrayBuffer]') {
                message = new TextDecoder().decode(data);
            }
        }
        this.showPopup(message || this.defaultErrorMessage, false);
    }

    private showPopup(message: string, success = true) {
        this.requestResult.success = success;
        this.requestResult.message = message;
        this.requestResult.showPopup = true;
    }
}

enum RequestTypes {
    'post',
    'postFile',
    'postArrayBuffer',
    'get',
    'head',
    'patch',
    'put',
    'delete',
}
