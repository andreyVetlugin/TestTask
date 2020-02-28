export default class EgissoForm {
    public id?: string;
    public name: string;
    public code: string;

    constructor(data: any) {
        this.id = data.id;
        this.name = data.title;
        this.code = data.code;
    }

    public fillFrom(data: EgissoForm) {
        this.id = data.id;
        this.name = data.name;
        this.code = data.code;
    }

    public toServerModel() {
        return {
            id: this.id,
            title: this.name,
            code: this.code,
        };
    }
}
