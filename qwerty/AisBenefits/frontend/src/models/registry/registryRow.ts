import { formatMoney, formatDate } from '../utils/stringBuilder';

export default class RegistryRow {
    public id: string;
    public number: string;
    public fio: string;
    public account: string;
    public baseSumm: number;
    public summ: string;
    public from: string;
    public to: string;
    public errorMessage: string;

    constructor(data: any) {
        this.id = data.id;
        this.number = data.personInfoNumber;
        this.fio = data.fio;
        this.account = data.account;
        this.summ = formatMoney(data.summ);
        this.from = formatDate(data.from);
        this.to = formatDate(data.to);
        this.baseSumm = data.baseSumm;
        this.errorMessage = data.errorMessage;
    }
}
