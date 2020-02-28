export function calculatePercents(base: string, summ: string, def = '0'): string {
    const floatBase = floatParse(base);
    // return floatBase === 0 ? '0' : Math.round(floatParse(summ) / floatBase * 100).toString();
    return floatBase === 0 ? def : (floatParse(summ) / floatBase * 100).toString();
}

export function calculateSumm(base: string, percent: string): string {
    return (floatParse(base) * floatParse(percent) / 100).toString();
}

export function multiplyStrings(items: string[]): string {
    if (!items || items.length < 0) {
        return '0.00';
    }
    let result = 1;
    for (const item of items) {
        result *= floatParse(item);
    }
    return result.toString();
}

export function summStrings(items: string[]): string {
    if (!items || items.length < 0) {
        return '0.00';
    }
    let result = 0;
    for (const item of items) {
        result += floatParse(item);
    }
    return result.toString();
}

export function substractStrings(left: string, right: string, min?: string): string {
    const floatLeft = floatParse(left);
    const floatRight = floatParse(right);
    let result = floatLeft - floatRight;
    if (min) {
        result = Math.max(floatParse(min), result);
    }
    return result.toString();
}

export function numericDiffStrings(left: string, right: string): number {
    const floatLeft = floatParse(left);
    const floatRight = floatParse(right);
    return Math.abs(floatLeft - floatRight);
}

export function floatParse(float: string): number {
    return float ? parseFloat(float.replace(/[^\-\d.]/g, '')) || 0 : 0;
}
