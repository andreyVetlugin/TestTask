import Validator from './validator';

export default class SnilsValidator implements Validator {
    public validate(value: string): boolean {
        value = (value || '').replace(/\D/g, '').substr(0, 11);
        if (value.length !== 11) {
            return false;
        }
        let sum = 0;
        for (let i = 0; i < 9; i++) {
            sum += parseInt(value[i], 10) * (9 - i);
        }
        sum = sum % 101 % 100;
        return value.substr(9, 2) === sum.toString();
    }
}
