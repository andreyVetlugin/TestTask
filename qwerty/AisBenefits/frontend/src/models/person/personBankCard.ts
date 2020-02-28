import { formatDate } from '../utils/stringBuilder';
import moment from 'moment';

export default class PersonBankCard {

    public personRootId: string;
    public id: string;
    public type: PersonBankCardType;
    public number: string;
    public account: string;
    public validThru: string;
    public validRemains: number;

    constructor(data: any) {
        this.personRootId = data.personRootId;
        this.id = data.id;
        this.type = data.type;
        this.account = '';
        this.number = '';
        this.validThru = '';
        this.validRemains = 999;
        switch (this.type) {
            case PersonBankCardType.Card:
                this.number = data.number;
                this.validThru = formatDate(data.validThru);
                this.validRemains = moment(data.validThru).diff(moment(), 'days');
                break;
            case PersonBankCardType.Account:
                this.account = data.number;
                break;
            default:
                this.account = data.number;
                break;
        }
    }
}

enum PersonBankCardType {
    Card,
    Account,
}
