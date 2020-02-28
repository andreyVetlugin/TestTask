import { calculatePercents, substractStrings } from '../utils/calculations';
import { formatMoney } from '../utils/stringBuilder';

export default class PensionRow {
    public selected: boolean;
    public number: number;
    public id: string;
    public fio: string;
    public currentPension: string;
    public newPension: string;
    public diff: string;

    constructor(data: any) {
        this.selected = true;
        this.id = data.gosPensionUpdateId;
        this.fio = data.fio;
        this.currentPension = formatMoney(data.currentPension);
        this.newPension = formatMoney(data.newPensionValue);
        this.diff = formatMoney(calculatePercents(
            this.currentPension, substractStrings(this.newPension, this.currentPension), '100',
        ));
        this.number = data.number;
    }
}
