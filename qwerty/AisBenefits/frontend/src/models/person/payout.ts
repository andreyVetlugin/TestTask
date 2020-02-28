import moment from 'moment';
import { formatMoney } from '@/models/utils/stringBuilder';

export default class Payout {
    public amount: string;
    public comment: string;
    public year: string;
    public month: string;
    public id: string;
    public personBankCardId: string;
    public personId: string;
    public date: string;

    constructor(payout: any) {
        this.amount = formatMoney(payout.amount);
        this.comment = payout.comment;
        this.date = payout.date;
        const date = moment(payout.date).locale('ru');
        if (date.isValid()) {
            this.year = date.format('YYYY');
            this.month = date.format('MMMM');
        } else {
            this.year = '';
            this.month = '';
        }
        this.id = payout.id;
        this.personBankCardId = payout.personBankCardId;
        this.personId = payout.personId;
    }

    public compare(payout: Payout) {
        return moment(this.date)
            .diff(payout.date, 'days');
    }
}
