import ReportRow from './reportRow';
import CardEditData from '../cardEditData';

export default class ReportModel {
    public PIForm = {
        number: new ReportRow<number>('Регистрационный номер'),
        fio: new ReportRow('ФИО'),
        sex: new ReportRow('Пол'),
        birthDate: new ReportRow('Дата рождения'),
        birthplace: new ReportRow('Место рождения'),
        snils: new ReportRow('СНИЛС'),
        docNumber: new ReportRow('Документ (серия)'),
        docSeria: new ReportRow('Документ (номер)'),
        Issuer: new ReportRow('Документ (кем выдан)'),
        IssueDate: new ReportRow('Документ (когда выдан)'),
        address: new ReportRow('Адрес'),
        phone: new ReportRow('Телефон'),
        employeeTypeId: new ReportRow('Тип получателя', true),
        payoutTypeId: new ReportRow('Тип выплаты', true),
        districtId: new ReportRow('Район получения пенсии', true),
        pensionCaseNumber: new ReportRow('Номер пенсионного дела'),
        pensionTypeId: new ReportRow('Тип пенсии', true),
        pensionEndDate: new ReportRow('Срок выплаты по..'),
        additionalPensionId: new ReportRow('Дополнительная пенсия', true),
    };
    public WIForm = {
        experience: new ReportRow('Общий стаж'),
        dismissalDate: new ReportRow('Дата увольнения'),
        approved: new ReportRow<boolean>('Стаж утвержден', true),
        docsSubmitDate: new ReportRow('Дата подачи документов'),
        docsDestinationDate: new ReportRow('Дата назначения доплат'),
        organizationId: new ReportRow('Организация', true),
        functionId: new ReportRow('Должность', true),
        startDate: new ReportRow('Дата начала'),
        endDate: new ReportRow('Дата окончания'),
    };
    public EPForm = {
        variantId: new ReportRow('Вариант расчета', true),
        uralMultiplier: new ReportRow<number>('Уральский коэффициент'),
        salary: new ReportRow<number>('Оклад'),
        salaryMultiplied: new ReportRow<number>('Оклад с уральским коэффициентом'),
        premium: new ReportRow<number>('Премия', true),
        materialSupport: new ReportRow<number>('Матпомощь - сумма'),
        materialSupportDivPerc: new ReportRow<number>('Матпомощь - %', true),
        perks: new ReportRow<number>('Надбавки  - сумма'),
        perksDivPerc: new ReportRow<number>('Надбавки  - %', true),
        vysluga: new ReportRow<number>('Выслуга - сумма'),
        vyslugaDivPerc: new ReportRow<number>('Выслуга - %', true),
        secrecy: new ReportRow<number>('Секретность - сумма'),
        secrecyDivPerc: new ReportRow<number>('Секретность - %', true),
        qualification: new ReportRow<number>('Классный чин - сумма '),
        qualificationDivPerc: new ReportRow<number>('Классный чин - %', true),
        ds: new ReportRow<number>('Денежное содержание'),
        dsPerc: new ReportRow<number>('% от Денежного содержания'),
        gosPension: new ReportRow<number>('Госпенсия'),
        extraPension: new ReportRow<number>('Дополнительная пенсия'),
        totalPension: new ReportRow<number>('Основная пенсия'),
        totalPensionAndExtraPay: new ReportRow<number>('МДС'),
        totalExtraPay: new ReportRow<number>('Доплата'),
    };
    public SolForm = {
        type: new ReportRow<number>('Вид решения', true),
        destination: new ReportRow('Дата назначения'),
        execution: new ReportRow('Дата исполнения'),
    };
    public PayoutForm = {
        lastPaySumm: new ReportRow<number>('Сумма последней выплаты'),
        lastPayDate: new ReportRow('Дата последней выплаты'),
        bankCardType: new ReportRow<number>('Счет', true),
        bankCardNumber: new ReportRow('Номер счета'),
    };

    constructor() {
        this.SolForm.type.variants = [
            { value: '0', title: 'Пересчитать' },
            { value: '1', title: 'Приостановить' },
            { value: '2', title: 'Возобновить' },
            { value: '3', title: 'Прекратить' },
        ];
        this.PayoutForm.bankCardType.variants = [
            { value: '0', title: 'Карта Сбербанка' },
            { value: '1', title: 'Счёт ЕМБ' },
        ];
        this.WIForm.approved.variants = [
            { value: 'true', title: 'Да' },
            { value: 'false', title: 'Нет' },
        ];
    }

    public setCardEditData(data: CardEditData) {
        this.PIForm.employeeTypeId.variants = data.employmentTypes;
        this.PIForm.payoutTypeId.variants = data.payoutTypes;
        this.PIForm.districtId.variants = data.districts;
        this.PIForm.pensionTypeId.variants = data.pensionTypes;
        this.PIForm.additionalPensionId.variants = data.additionalPensionTypes;
    }

    public setOrganizations(organizations: []) {
        this.WIForm.organizationId.variants = organizations;
    }

    public setFunctions(functions: []) {
        this.WIForm.functionId.variants = functions;
    }

    public setExtraPayVariants(extraPayVariants: []) {
        this.EPForm.variantId.variants = extraPayVariants;
    }

    public setAll(key: string, value: boolean) {
        const block = (this as any)[key];
        for (const i in block) {
            if (block[i].selected !== undefined) {
                block[i].selected = value;
            }
        }
    }

    public toServerModel() {
        const result = {} as any;
        const errors = [] as string[];
        Object.entries(this).forEach((block) => {
            const resBlock = {} as any;
            Object.entries(block[1]).forEach((elem) => {
                try {
                    resBlock[elem[0]] = (elem[1] as any).toServerModel();
                } catch (err) {
                    errors.push(err);
                }
            });
            result[block[0]] = resBlock;
        });
        if (errors.length) {
            throw errors;
        }
        return result;
    }
}
