import RegistryRow from './registryRow';
import { formatMoney, formatDate } from '../utils/stringBuilder';

export default class Registry {

    public registry = {
        id: '',
        date: formatDate(new Date().toISOString()),
        initDate: formatDate(new Date().toISOString()),
        isCompleted: false,
    };

    public rows: RegistryRow[];
    public count: number;
    public summ: string;
    public canComplete: boolean;

    constructor(data: any) {
        if (data.reestrElements && data.reestrElements.length) {
            this.rows = data.reestrElements.map((el: any) => {
                return new RegistryRow(el);
            });
        } else {
            this.rows = [];
        }

        if (data.reestr) {
            this.registry.id = data.reestr.id;
            this.registry.date = formatDate(data.reestr.date);
            this.registry.initDate = formatDate(data.reestr.initDate);
            this.registry.isCompleted = data.reestr.isCompleted;
        }
        this.count = parseInt(data.numberOfElements, 10) || 0;
        this.summ = formatMoney(data.summTotal);
        this.canComplete = data.canComplete;
    }

    public fillFrom(data: Registry) {
        this.registry.id = data.registry.id;
        this.registry.date = data.registry.date;
        this.registry.initDate = data.registry.initDate;
        this.registry.isCompleted = data.registry.isCompleted;

        this.fillRows(data.rows);

        this.count = data.count;
        this.summ = data.summ;
        this.canComplete = data.canComplete;
    }

    public fillRows(rows: RegistryRow[]) {
        while (this.rows.length) {
            this.rows.pop();
        }
        for (const row of rows) {
            this.rows.push(row);
        }
    }
}
