import { formatMoney } from '../utils/stringBuilder';
import { substractStrings } from '../utils/calculations';

export default class RecountRow {
    public month: string;
    public current: string;
    public full: string;
    public diff: string;

    constructor(data: any) {
        this.month = data.month;
        this.current = data.current;
        this.full = data.full;
        this.diff = data.diff;
    }

    public toServerModel() {
        return this;
    }
}
