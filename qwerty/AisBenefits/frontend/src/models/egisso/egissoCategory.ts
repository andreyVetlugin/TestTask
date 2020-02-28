export default class EgissoCategory {
    public measureUnitId: string;
    public categoryId: string;
    public egissoId: string;
    public name: string;

    constructor(data: any) {
        this.categoryId = data.kpCodeId;
        this.measureUnitId = data.measureUnitId;
        this.egissoId = data.egissoId;
        this.name = '';
    }

    public toServerModel() {
        return {
            categoryId: this.categoryId,
            measureUnitId: this.measureUnitId,
            egissoId : this.egissoId,
            value: 0,
        };
    }
}
