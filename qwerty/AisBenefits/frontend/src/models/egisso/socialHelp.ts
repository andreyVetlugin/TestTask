import EgissoCategory from './egissoCategory';

export default class SocialHelp {
    public id?: string;
    public name: string;
    public code: string;
    public formId: string;
    public periodId: string;
    public egissoId: string;
    public categories: EgissoCategory[];
    public isNeed: string;
    public isPayment: string;

    constructor(data: any) {
        if (!data || !data.privilege) {
            data = {
                privilege: {},
            };
        }
        this.id = data.privilege.id;
        this.name = data.privilege.title;
        this.code = data.privilege.egissoCode;
        this.formId = data.privilege.provisionFormId;
        this.periodId = data.privilege.periodTypeId;
        this.egissoId = data.privilege.egissoId;
        this.categories = (data.categories || []).map((el: any) => new EgissoCategory(el)) || [];
        this.isNeed = data.privilege.usingNeedCriteria ? 'true' : 'false';
        this.isPayment = data.privilege.monetization ? 'true' : 'false';
    }

    public fillFrom(data: SocialHelp) {
        this.id = data.id;
        this.name = data.name;
        this.code = data.code;
        this.formId = data.formId;
        this.periodId = data.periodId;
        this.egissoId = data.egissoId;
        this.categories = data.categories;
        this.isNeed = data.isNeed;
        this.isPayment = data.isPayment;
    }

    public toServerModel() {
        return {
            id: this.id,
            title: this.name,
            egissoCode: this.code,
            provisionFormId: this.formId,
            periodTypeId: this.periodId,
            egissoId: this.egissoId,
            categories: this.categories.map((el) => el.toServerModel()),
            usingNeedCriteria: this.isNeed === 'true',
            monetization: this.isPayment === 'true',
        };
    }
}
