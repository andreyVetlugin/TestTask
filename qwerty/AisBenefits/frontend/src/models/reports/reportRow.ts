import { floatParse } from '../utils/calculations';

export default class ReportRow<T = string> {
    public selected: boolean = false;
    public title: string;
    public canFiltered: boolean;
    public filtered: boolean = false;
    public filterRule: FilterRules = FilterRules.equal;
    public filterValue: string;
    public variants: Array<{ value: string, title: string }> = [];
    private type = typeof ({} as T);

    constructor(
        title = '',
        canFiltered = false) {
        this.title = title;
        this.canFiltered = canFiltered;
        this.filterValue = '';
    }

    public toServerModel() {
        if (this.filtered && !this.filterValue) {
            throw this.getEmptyFilterMessage();
        }
        const result = {
            isShow: this.selected,
            isFiltered: this.filtered,
            filterType: this.filterRule,
        } as any;
        switch (this.type) {
            case 'boolean':
                result.value = this.filterValue === 'true';
                break;
            case 'number':
                result.value = floatParse(this.filterValue);
                break;
            default:
                result.value = this.filterValue;
        }
        return result;
    }

    public isNumeric() {
        return this.type === 'number';
    }

    private getEmptyFilterMessage() {
        return 'При выборе значения "' + this.title + '" не был установлен фильтр.';
    }
}

export enum FilterRules {
    equal,
    greater,
    lower,
    notEqual,
    // greaterOrEquals,
    // lowerOrEquals,
    // between,
    // array,
}
