import moment from 'moment';

export function formatMoney(money: number | string, digits?: number) {
    let first = '0';
    let second = '';
    if (digits === undefined || digits < 0) {
        digits = 2;
    }
    if (money) {
        first = '';
        if (typeof money === 'string') {
            money = parseFloat(money.replace(/[^\-\d.]/g, ''));
        }
        const rounding = Math.pow(10, digits || 1);
        const parts = (Math.round(money * rounding) / rounding).toString().split('.');
        const count = parts[0].length;
        for (let i = 1; i <= count; i++) {
            first = parts[0][count - i] + first;
            if (i % 3 === 0) {
                first = ' ' + first;
            }
        }
        second = parts[1] || '';
    }
    let result = first.replace('- ', '-').trim();
    if (digits > 0) {
        while (second.length < digits) {
            second += '0';
        }
        result += `.${second.substring(0, digits)}`;
    }
    return result;
}

export function formatDate(date: string, format?: string) {
    if (!date) {
        return '';
    }
    const result = moment(date);
    return result.isValid()
        ? result.locale('ru').format(format || 'DD.MM.YYYY')
        : '';
}

export function ruDateToISO(date: string): string {
    if (/^[\d]{2}.[\d]{2}.[\d]{4}$/.test(date)) {
        return date.split('.').reverse().join('-');
    }
    return '';
}

export function ageToString(y: number, m: number, d: number, separator?: string) {
    const age = [];
    switch (true) {
        case (y <= 0):
            break;
        case ((y === 1) || (y > 20 && y % 10 === 1)):
            age.push(`${y} год`);
            break;
        case ((y > 1 && y < 5) || (y > 20 && (y % 10 > 1 && y % 10 < 5))):
            age.push(`${y} года`);
            break;
        default:
            age.push(`${y} лет`);
            break;
    }
    switch (true) {
        case (m <= 0 || m > 12):
            break;
        case (m === 1):
            age.push(`${m} месяц`);
            break;
        case (m < 5):
            age.push(`${m} месяца`);
            break;
        default:
            age.push(`${m} месяцев`);
            break;
    }
    switch (true) {
        case (d <= 0):
            break;
        case ((d === 1) || (d > 20 && d % 10 === 1)):
            age.push(`${d} день`);
            break;
        case ((d > 1 && d < 5) || (d > 20 && (d % 10 > 1 && d % 10 < 5))):
            age.push(`${d} дня`);
            break;
        default:
            age.push(`${d} дней`);
            break;
    }

    return age.join(separator || ' ');
}
