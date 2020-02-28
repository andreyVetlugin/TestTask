export default class EgissoPeriod {
    public id?: string;
    public name: string;
    public number: string;
    public code: string;

    constructor(data: any) {
        this.id = data.id;
        this.name = data.value;
        this.number = (data.ppNumber || '1').toString();
        this.code = data.positionCode;
    }

    public fillFrom(data: EgissoPeriod) {
        this.id = data.id;
        this.name = data.name;
        this.number = data.number;
        this.code = data.code;
    }

    public toServerModel() {
        return {
            id: this.id,
            value: this.name,
            ppNumber: parseInt(this.number, 10),
            positionCode: this.code,
        };
    }
}
