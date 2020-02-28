export default class MeasureUnit {
    public id?: string;
    public name: string;
    public shortName: string;
    public number: string;
    public code: string;
    public okayCode: string;
    public type: string; // целое или десятичное

    constructor(data: any) {
        this.id = data.id;
        this.name = data.title;
        this.shortName = data.shortTitle;
        this.number = (data.ppNumber || '1').toString();
        this.code = data.positionCode;
        this.okayCode = data.okeiCode;
        this.type = data.isDecimal ? '1' : '0';
    }

    public fillFrom(data: MeasureUnit) {
        this.id = data.id;
        this.name = data.name;
        this.shortName = data.shortName;
        this.number = data.number;
        this.code = data.code;
        this.okayCode = data.okayCode;
        this.type = data.type;
    }

    public toServerModel() {
        return {
            id: this.id,
            title: this.name,
            shortTitle: this.shortName,
            ppNumber: parseInt(this.number, 10),
            positionCode: this.code,
            okeiCode: this.okayCode,
            isDecimal: this.type === '1',
        };
    }
}
