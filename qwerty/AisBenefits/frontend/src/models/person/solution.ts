import { formatDate, formatMoney } from '@/models/utils/stringBuilder';

export default class Solution {
    public comment: string;
    public dSperc: number;
    public destination: string;
    public ds: string;
    public mds: string;
    public execution: string;
    public id: string;
    public solutionTypeStr: string;
    public totalExtraPay: string;
    public totalPension: string;
    public allowDelete: boolean;

    constructor(data: any) {
        this.id = data.id;
        this.destination = formatDate(data.destination);
        this.execution = formatDate(data.execution);
        this.solutionTypeStr = data.solutionType_str;
        this.comment = data.comment;
        this.totalExtraPay = formatMoney(data.totalExtraPay);
        this.totalPension = formatMoney(data.totalPension);
        this.ds = formatMoney(data.ds);
        this.mds = formatMoney(data.mds);
        this.dSperc = data.dSperc;
        this.allowDelete = data.allowDelete;
    }
}
