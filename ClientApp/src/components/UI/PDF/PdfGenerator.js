import { jsPDF } from "jspdf";
import autoTable from 'jspdf-autotable';
import moment from 'moment';

export const PdfGenerator = ({ body, header, entireAudio }) => { 
    if (!entireAudio) {
        Array.prototype.move = function (from, to) {
            this.splice(to, 0, this.splice(from, 1)[0]);
        };

        const doc = new jsPDF();
        var now = moment(new Date());
        now.format("dddd, MMMM YYYY, h:mm:ss");

        doc.text(`Test Harness Report from: ${now}`, 10, 10)
        body.map((x, index) => {
            let bodyData = [];
            x.baseReport.forEach((element, index) => {
                var item = Object.values(element); 
                // item.shift();
                item.move(1, 0);
                item.move(2, 1);
                item.move(3, 2);
                item.move(4, 3);
                item.move(6, 5);
                bodyData.push(item);
            });
 
            let footerData = Object.values(x.summaryReport);
            footerData[0] = `Tested Users: ${footerData[0]} `;
            footerData[1] = `Percentage Call Duration: ${footerData[1]} `;
            footerData[2] = `Api Call Status: ${footerData[2]} `;
            footerData[3] = `Used Audio: ${footerData[3]} `;
            footerData[4] = `Server: ${footerData[4]}`;

            footerData.unshift(`Test Harness Summary: ${index + 1}`)
            //  [].concat.apply([], bodyData);

            doc.autoTable({
                head: [header],
                body: bodyData,
                rowPageBreak: 'auto',
                theme: 'grid',
                footStyles: { Color: '#6AB931' },
                foot: [footerData]
            })
        })

        doc.save(`test~report~date~${now.format("dddd, MMMM YYYY, h:mm:ss")}.pdf`);
    } else {
        header = ["User Name", "Channel Chunking Details"];
        Array.prototype.move = function (from, to) {
            this.splice(to, 0, this.splice(from, 1)[0]);
        };

        const doc = new jsPDF();
        var now = moment(new Date());
        now.format("dddd, MMMM YYYY, h:mm:ss");

        doc.text(`Test Harness Report from: ${now}`, 10, 10)
        body.map((x, index) => {
            let bodyData = [];
            x.baseReport.forEach((element) => {
                var item = Object.values(element); 
                // item.move(1, 0);
                // item.move(7, 1);
                // item.move(5, 2);

                var newArr = [];
            
                item[1].forEach((channel, channelIndex)=>{
                    channel.chunks.map( (chunk, chunckIndex) => {  
                        newArr.push(`\nChannel No: ${channelIndex}, Used Audio: ${channel.name}, Call Successful: ${channel.apiCallStatus}
                        Chunk Number: ${chunk.chunkNumber}, ChunkRangeTaken: ${chunk.chunkRangeTaken}Mb, ChunkTimeCallStart: ${chunk.chunkTimeCallStart}, ChunkTimeCallEnd: ${chunk.chunkTimeCallEnd}, ChunkTimeCallDuration: ${chunk.chunkTimeCallDuration}, ResponseChunkSize: ${chunk.responseChunkSize}Mb\n`);
                    })
                });

                 item[1] = newArr;
               
                bodyData.push(item);
            }); 

            let footerData = Object.values(x.summaryReport);
            let footerDetail = [];
            footerDetail.unshift(`Test Harness Summary: ${index + 1}`);
            footerDetail.push(`Tested Users: ${footerData[0]}, Server: ${footerData[4]}`); 

            doc.autoTable({
                head: [header],
                body: bodyData,
                rowPageBreak: 'auto',
                theme: 'grid',
                footStyles: { Color: '#6AB931' },
                foot: [footerDetail]
            })
        })

        doc.save(`test~report~date~${now.format("dddd, MMMM YYYY, h:mm:ss")}.pdf`);
    }
}
