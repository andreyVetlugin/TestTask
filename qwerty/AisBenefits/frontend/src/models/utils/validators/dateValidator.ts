import Validator from './validator';
import moment from 'moment';

export default class DateValidator implements Validator {
    public validate(value: string): boolean {
        return moment(value, 'DD.MM.YYYY').isValid();
    }
}
