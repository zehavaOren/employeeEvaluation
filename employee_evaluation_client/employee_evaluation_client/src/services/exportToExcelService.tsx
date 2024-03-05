import * as XLSX from 'xlsx';
import saveAs from 'file-saver';

export const exportToExcelService = {

     exportToExcel : (columns, data, filename) => {
        const worksheetData = data.map(item => {
            const row = {};
            columns.forEach(column => {
                row[column.title] = item[column.key] || ''; 
            });
            return row;
        });

        const worksheet = XLSX.utils.json_to_sheet(worksheetData);
        const workbook = XLSX.utils.book_new();
        XLSX.utils.book_append_sheet(workbook, worksheet, 'Sheet1');

        const excelBuffer = XLSX.write(workbook, { bookType: 'xlsx', type: 'array' });
        saveAs(new Blob([excelBuffer], { type: 'application/octet-stream' }), `${filename}.xlsx`);
    },

};


