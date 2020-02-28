import { formatDate, ageToString } from '@/models/utils/stringBuilder';

export default class WorkInfo {
    public id: string;
    public startDate: string;
    public endDate: string;
    public age: string;
    public organizationId: string;
    public organizationName: string;
    public functionId: string;
    public function: string;

    constructor(work: any) {
        this.id = work.rootId;
        this.startDate = formatDate(work.startDate);
        this.endDate = formatDate(work.endDate);
        this.age = ageToString(work.ageYears as number,
            work.ageMonths as number,
            work.ageDays as number);
        this.organizationId = work.organizationId;
        this.organizationName = work.organizationName;
        this.functionId = work.functionId;
        this.function = work.function;
    }

    public toSendModel() {
        return {
            rootId: this.id,
            startDate: this.startDate,
            endDate: this.endDate,
            organizationId: this.organizationId,
            organization: this.organizationName,
            functionId: this.functionId,
            function: this.function,
        };
    }
}
