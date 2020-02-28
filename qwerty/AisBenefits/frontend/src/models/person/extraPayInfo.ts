import { formatMoney } from '@/models/utils/stringBuilder';
import { floatParse } from '../utils/calculations';

export default class ExtraPay {

    public static fieldTitleLocale = {
        variantid: 'Вариант расчёта',
        uralmultiplier: 'Уральский к-т',
        salary: 'Оклад',
        matsupportmultiplier: 'Мат. помощь',
        perks: 'Надбавки',
        vysluga: 'Выслуга',
        vyslugadivperc: '',
        secrecy: 'Секретность',
        secrecydivperc: '',
        qualification: 'Классный чин',
        premiumperc: '% премии',
        gospension: 'Госпенсия',
        extrapension: 'Доп. пенсия',
        premium: 'Премия',
    };

    public ds: string;
    public dsPerc: string;
    public extraPension: string;
    public gosPension: string;
    public materialSupport: string;
    public matSupportMultiplier: string;
    public perks: string;
    public perksDivPerc: string;
    public premium: string;
    public premiumPerc: string;
    public qualification: string;
    public qualificationDivPerc: string;
    public salary: string;
    public salaryMultiplied: string;
    public secrecy: string;
    public secrecyDivPerc: string;
    public totalExtraPay: string;
    public totalPension: string;
    public totalPensionAndExtraPay: string;
    public uralMultiplier: string;
    public variantId: string;
    public variantNumber: string;
    public vysluga: string;
    public vyslugaDivPerc: string;

    constructor(extraPay: any) {
        this.variantId = extraPay.variantId || '00000000-0000-0000-0000-000000000000';

        this.ds = formatMoney(extraPay.ds);
        this.dsPerc = formatMoney(extraPay.dsPerc, 0);
        this.extraPension = formatMoney(extraPay.extraPension);
        this.gosPension = formatMoney(extraPay.gosPension);
        this.materialSupport = formatMoney(extraPay.materialSupport);
        this.matSupportMultiplier = (extraPay.matSupportMultiplier || 0).toString();
        this.perks = formatMoney(extraPay.perks);
        this.perksDivPerc = formatMoney(extraPay.perksDivPerc, 0);
        this.premium = formatMoney(extraPay.premium);
        this.premiumPerc = formatMoney(extraPay.premiumPerc, 0);
        this.qualification = formatMoney(extraPay.qualification);
        this.qualificationDivPerc = formatMoney(extraPay.qualificationDivPerc, 0);
        this.salary = formatMoney(extraPay.salary);
        this.salaryMultiplied = formatMoney(extraPay.salaryMultiplied);
        this.secrecy = formatMoney(extraPay.secrecy);
        this.secrecyDivPerc = formatMoney(extraPay.secrecyDivPerc, 0);
        this.totalExtraPay = formatMoney(extraPay.totalExtraPay);
        this.totalPension = formatMoney(extraPay.totalPension);
        this.totalPensionAndExtraPay = formatMoney(extraPay.totalPensionAndExtraPay);
        this.uralMultiplier = formatMoney(extraPay.uralMultiplier);
        this.variantNumber = formatMoney(extraPay.variantNumber);
        this.vysluga = formatMoney(extraPay.vysluga);
        this.vyslugaDivPerc = formatMoney(extraPay.vyslugaDivPerc, 0);
    }

    public toSendModel(personRootId: string): {} {
        const result = {
            personRootId,
            variantId: this.variantId,
            uralMultiplier: floatParse(this.uralMultiplier),
            salary: floatParse(this.salary),
            premium: floatParse(this.premium),
            materialSupport: floatParse(this.materialSupport),
            perks: floatParse(this.perks),
            vysluga: floatParse(this.vysluga),
            secrecy: floatParse(this.secrecy),
            qualification: floatParse(this.qualification),
            gosPension: floatParse(this.gosPension),
            extraPension: floatParse(this.extraPension),
            ds: floatParse(this.ds),
            totalExtraPay: floatParse(this.totalExtraPay),
            totalPension: floatParse(this.totalPension),
            totalPensionAndExtraPay: floatParse(this.totalPensionAndExtraPay),
        } as any;
        for (const entry of Object.entries(result)) {
            if (entry[1] < 0) {
                result[entry[0]] = 0;
            }
        }
        return result;
    }
}
