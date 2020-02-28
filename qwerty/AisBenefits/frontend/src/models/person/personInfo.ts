import WorkInfo from './workInfo';
import ExtraPay from './extraPayInfo';
import Payout from './payout';
import { formatDate, formatMoney, ageToString } from '@/models/utils/stringBuilder';
import PersonBankCard from './personBankCard';
import Solution from './solution';
import InputMasker from '../utils/inputMasker';

export default class PersonInfo {

    public static fieldTitleLocale = {
        additionalpensionid: 'Дополнительная пенсия',
        address: 'Адрес',
        birthdate: 'Дата рождения',
        birthplace: 'Место рождения',
        codeegisso: '',
        districtId: 'Район получения пенсии',
        docnumber: 'Номер документа',
        docseria: 'Серия документа',
        doctypeid: 'Тип документа',
        email: 'Электронная почта',
        employeetypeid: 'Тип',
        issuedate: 'Когда выдан',
        issuer: 'Кем выдан',
        middlename: 'Отчество',
        name: 'Имя',
        number: 'Номер по реестру',
        pensionenddate: 'Срок выплаты',
        pensiontypeid: 'Тип пенсии',
        payouttypeid: 'Тип выплаты',
        pensioncasenumber: 'Номер пенсионного дела',
        phone: 'Телефон',
        rootid: '',
        sex: 'Пол',
        snils: 'СНИЛС',
        surname: 'Фамилия',
    };

    public id: string;
    public employeeType: string;
    public employType: string;
    public extraPayVariant: string;

    public number: string;
    public registrationDate: string;
    public pensionCaseNumber: string;
    public firstName: string;
    public secondName: string;
    public surName: string;
    public birthPlace: string;
    public birthDate: string;
    public sex: string;
    public snils: string;
    public phone: string;
    public email: string;
    public pension: string;
    public yearsBonusPension: string;
    public bankAccountNumber: string;
    public ksp: string;
    public district: string;
    public address: string;
    public documentType: string;
    public documentcodeEgisso: string;
    public documentNumber: string;
    public documentSeria: string;
    public documentIssuer: string;
    public documentIssueDate: string;
    public salary: string;
    public additionalPension: string;
    public pensionType: string;
    public payoutType: string;
    public pensionEndDate: string;
    public approved: boolean;
    public paused: boolean;

    public conditionsBonus: string;
    public conditionsBonusInPercents: string;
    public yearsBonus: string;
    public yearsBonusInPercents: string;
    public secretDataBonus: string;
    public secretDataBonusInPercents: string;
    public municipalBonus: string;
    public municipalBonusInPercents: string;
    public premiumBonus: string;
    public materialHelp: string;

    public worksApproved: boolean = false;
    public worksAge: string = '';
    public docsSubmitDate: string;
    public docsDestinationDate: string;
    public works: WorkInfo[];
    public payouts: Payout[];
    public solutions: Solution[];
    public extraPay: ExtraPay;
    public bankCard: PersonBankCard;

    /*  send */
    public additionalPensionId: string;
    public districtId: string;
    public documentTypeId: string;
    public employeeTypeId: string;
    public pensionTypeId: string;
    public payoutTypeId: string;
    /* /send */



    constructor(person?: any) {
        if (!person) {
            person = {};
        }

        this.id = person.rootId;
        this.employeeType = person.employeeType;
        this.employType = person.employType;
        this.extraPayVariant = person.extraPayVariant;

        this.number = person.number >= 0 ? person.number.toString() : '';
        this.registrationDate = formatDate(person.registrationDate);
        this.pensionCaseNumber = person.pensionCaseNumber >= 0 ? person.pensionCaseNumber.toString() : '';
        this.firstName = person.name;
        this.secondName = person.middleName;
        this.surName = person.surName;
        this.birthPlace = person.birthplace;
        this.birthDate = formatDate(person.birthDate);
        this.sex = (person.sex || 'М').toUpperCase();
        this.snils = person.snils;
        this.phone = (person.phone || '').replace(/\D/g, '').replace(/^8/, '');
        this.email = person.email;
        this.pension = formatMoney(person.pension);
        this.yearsBonusPension = formatMoney(person.yearsBonusPension);
        this.bankAccountNumber = person.bankAccountNumber;
        this.ksp = person.ksp;
        this.district = person.district;
        this.address = person.address;
        this.documentType = '03';
        this.documentcodeEgisso = person.codeEgisso;
        this.documentNumber = person.docNumber;
        this.documentSeria = person.docSeria;
        this.documentIssuer = person.issuer;
        this.documentIssueDate = formatDate(person.issueDate);

        this.salary = formatMoney(person.salary);
        this.additionalPension = person.additionalPension;
        this.pensionType = person.pensionType;
        this.payoutType = person.payoutType;
        this.pensionEndDate = formatDate(person.pensionEndDate);

        this.conditionsBonus = formatMoney(person.conditionsBonus);
        this.conditionsBonusInPercents = (person.conditionsBonusInPercents || 0).toString();
        this.yearsBonus = formatMoney(person.yearsBonus);
        this.yearsBonusInPercents = (person.yearsBonusInPercents || 0).toString();
        this.secretDataBonus = formatMoney(person.secretDataBonus);
        this.secretDataBonusInPercents = (person.secretDataBonusInPercents || 0).toString();
        this.municipalBonus = formatMoney(person.municipalBonus);
        this.municipalBonusInPercents = (person.municipalBonusInPercents || 0).toString();
        this.premiumBonus = formatMoney(person.premiumBonus);
        this.materialHelp = formatMoney(person.materialHelp);
        this.works = [];
        this.payouts = [];
        this.solutions = [];
        this.extraPay = new ExtraPay({});
        this.bankCard = new PersonBankCard({});
        if (person.works) {
            for (const work of person.works) {
                this.works.push(new WorkInfo(work));
            }
        }

        this.approved  = person.approved;
        this.paused  = person.paused;
        this.worksApproved = person.worksApproved;
        this.docsSubmitDate = formatDate(person.docsSubmitDate);
        this.docsDestinationDate = formatDate(person.docsDestinationDate);

        this.additionalPensionId = person.additionalPensionId;
        this.districtId = person.districtId;
        this.documentTypeId = person.codeEgisso;
        this.employeeTypeId = person.employeeTypeId;
        this.pensionTypeId = person.pensionTypeId;
        this.payoutTypeId = person.payoutTypeId;
    }

    public setWorkAge(y: number, m: number, d: number) {
        this.worksAge = ageToString(y, m, d);
    }

    public clone() {
        return new PersonInfo().fillFrom(this);
    }

    public fillFrom(data: PersonInfo) {
        this.id = data.id;
        this.employeeType = data.employeeType;
        this.employType = data.employType;
        this.number = data.number;
        this.pensionCaseNumber = data.pensionCaseNumber;
        this.firstName = data.firstName;
        this.secondName = data.secondName;
        this.surName = data.surName;
        this.birthPlace = data.birthPlace;
        this.birthDate = data.birthDate;
        this.sex = data.sex;
        this.snils = data.snils;
        const masker = new InputMasker('+7 (999) 999-99-99', false);
        this.phone = data.phone ? masker.mask(data.phone) : '';
        this.email = data.email;
        this.pension = data.pension;
        this.yearsBonusPension = data.yearsBonusPension;
        this.bankAccountNumber = data.bankAccountNumber;
        this.ksp = data.ksp;
        this.district = data.district;
        this.address = data.address;
        this.documentType = data.documentType;
        this.documentcodeEgisso = data.documentcodeEgisso;
        this.documentNumber = data.documentNumber;
        this.documentSeria = data.documentSeria;
        this.documentIssuer = data.documentIssuer;
        this.documentIssueDate = data.documentIssueDate;
        this.salary = data.salary;
        this.additionalPension = data.additionalPension;
        this.pensionType = data.pensionType;
        this.payoutType = data.payoutType;
        this.pensionEndDate = data.pensionEndDate;
        this.conditionsBonus = data.conditionsBonus;
        this.conditionsBonusInPercents = data.conditionsBonusInPercents;
        this.yearsBonus = data.yearsBonus;
        this.yearsBonusInPercents = data.yearsBonusInPercents;
        this.secretDataBonus = data.secretDataBonus;
        this.secretDataBonusInPercents = data.secretDataBonusInPercents;
        this.municipalBonus = data.municipalBonus;
        this.municipalBonusInPercents = data.municipalBonusInPercents;
        this.premiumBonus = data.premiumBonus;
        this.materialHelp = data.materialHelp;
        this.works = data.works;
        this.payouts = data.payouts;
        this.solutions = data.solutions;
        this.extraPay = data.extraPay;
        this.bankCard = data.bankCard;

        this.worksAge = data.worksAge;
        this.worksApproved = data.worksApproved;
        this.docsSubmitDate = data.docsSubmitDate;
        this.docsDestinationDate = data.docsDestinationDate;

        this.additionalPensionId = data.additionalPensionId;
        this.districtId = data.districtId;
        this.documentTypeId = data.documentTypeId;
        this.employeeTypeId = data.employeeTypeId;
        this.pensionTypeId = data.pensionTypeId;
        this.payoutTypeId = data.payoutTypeId;
    }

    public toServerModel() {
        if (this.phone.replace(/\D/g, '').length < 11) {
            this.phone = '';
        }
        return {
            additionalPensionId: this.additionalPensionId,
            address: this.address,
            birthDate: this.birthDate,
            birthplace: this.birthPlace,
            codeEgisso: this.documentcodeEgisso,
            districtId: this.districtId,
            docNumber: this.documentNumber,
            docSeria: this.documentSeria,
            docTypeId: this.documentTypeId,
            email: this.email,
            employeeTypeId: this.employeeTypeId,
            issueDate: this.documentIssueDate,
            issuer: this.documentIssuer,
            middleName: this.secondName,
            name: this.firstName,
            number: this.number,
            pensionCaseNumber: this.pensionCaseNumber,
            pensionEndDate: this.pensionEndDate,
            pensionTypeId: this.pensionTypeId,
            payoutTypeId: this.payoutTypeId,
            phone: this.phone,
            rootId: this.id,
            sex: this.sex,
            snils: this.snils,
            surName: this.surName,
            variantId: this.extraPay.variantId,
        };
    }
}
