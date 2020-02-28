import { ageToString } from '@/models/utils/stringBuilder';

export default class MdsPercents {
    public id: string;
    public age: string;
    public amount: string;
    public allowEdit: boolean;

    constructor(percs: any) {
        this.id = percs.id;
        this.amount = percs.amount;
        this.age = ageToString(percs.ageYears as number,
            percs.ageMonths as number,
            0);
        this.allowEdit = percs.allowEdit;
    }
}
