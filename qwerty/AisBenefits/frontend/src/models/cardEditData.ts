export default class CardEditData {
    [key: string]:
    Array<{ value: string, title: string }> | number | ((property: string, value: string) => string);

    public additionalPensionTypes: Array<{ value: string, title: string }>;
    public districts: Array<{ value: string, title: string }>;
    public employmentTypes: Array<{ value: string, title: string }>;
    public pensionTypes: Array<{ value: string, title: string }>;
    public payoutTypes: Array<{ value: string, title: string }>;
    public personDocumentTypes: Array<{ value: string, title: string }>;
    public sexList = [{ value: 'М', title: 'М' }, { value: 'Ж', title: 'Ж' }];
    public documentIssuers: Array<{ value: string, title: string }>;
    public nextNumber: number;

    constructor(data?: any) {
        if (!data) {
            data = {};
        }

        const parseKeyValueArray = (raw: any): Array<{ value: string, title: string }> => {
            const array = [];
            if (raw) {
                for (const i in raw) {
                    if (typeof raw[i] === 'string') {
                        array.push({
                            value: i.toString(),
                            title: raw[i],
                        });
                    }
                }
            }
            return array;
        };

        this.additionalPensionTypes = parseKeyValueArray(data.additionalPensionTypes);
        this.districts = parseKeyValueArray(data.districts);
        this.employmentTypes = parseKeyValueArray(data.employmentTypes);
        this.pensionTypes = parseKeyValueArray(data.pensionTypes);
        this.payoutTypes = parseKeyValueArray(data.payoutTypes);
        this.personDocumentTypes = parseKeyValueArray(data.personDocumentTypes);
        this.documentIssuers = parseKeyValueArray(data.documentIssuers);

        this.nextNumber = parseInt(data.nextNumber, 10);
    }

    public getTitleByValue(property: string, value: string) {
        if (!property || !value) {
            return '';
        }

        const list = this[property];
        if (!list || typeof list === 'number' || typeof list === 'function') {
            return '';
        }

        const elem = list.find((el) => el.value === value);

        return elem ? elem.title : '';
    }

    public getValueByTitle(property: string, title: string) {
        if (!property) {
            return '';
        }

        const list = this[property];
        if (!list || typeof list === 'number' || typeof list === 'function') {
            return '';
        }

        const elem = list.find((el) => el.title === title);
        return elem ? elem.value : new CardEditDefaultValues()[property];
    }
}

// tslint:disable-next-line:max-classes-per-file
class CardEditDefaultValues {
    [key: string]: string;
    public additionalPensionTypes = '00000000-0000-0000-0000-000000000000';
    public districts = '00000000-0000-0000-0000-000000000000';
    public employmentTypes = 'aa22bcbd-f41e-4373-9cd5-43dab1c921ff';
    public pensionTypes = '0d807f8b-909f-4460-9fbd-4cab49b854c2';
    public payoutTypes = '045c8ef4-b83c-46d8-9b48-60eb9119714a';
    public personDocumentTypes = '03';
}
